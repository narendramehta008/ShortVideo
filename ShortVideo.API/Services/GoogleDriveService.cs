using BaseLibrary.Constants;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ShortVideo.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Google.Apis.Drive.v3.FilesResource;

namespace ShortVideo.API.Services
{
    public class GoogleDriveFolderId
    {
        public const string RootParentFolder = "11PfREpvFuSUeMbclw4_pems2X4vngXVA";
        public const string DefaultImageFolder = "1I60sZ7lBr1KZayLVr9fModEGQ-7KhAwy";
        public const string DefaultImageParentFolder = "1nKnuL3qHT1CKw_v6wtX8Ca08HAh_6bQF";

        public const string DefaultVideoFolder = "1MHlsm_4rZb3vc1oBaOMZuZyJdMGLZOMP";
        public const string DefaultVideoParentFolder = "1nNG8Ow-cBBd-3xr3oYugkUiSlh6l21iM";

        public const string DefaultFileFolder = "1Vn1Qa7L4j3y8SyErG045-ZcugRQWkLKG";
        public const string DefaultFileParentFolder = "16W8ahy1mBUkqhskBQCL6NqwXEBVbYpbJ";

    }
    public interface IGoogleDriveService
    {
        Task<Dictionary<string, string>> GetFiles(int pageSize = 1000, string folderId = GoogleDriveFolderId.RootParentFolder);
        Task<Dictionary<string, string>> GetFolders(int pageSize = 1000, string folderId = GoogleDriveFolderId.RootParentFolder);
        Task<Dictionary<string, string>> GetFilesOrFolders(string query, int pageSize = 1000);
        /// <summary>
        /// It checks all the folders inside root folder including sub folders
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        Task<KeyValuePair<string, string>> GetFolderIdByName(string folderName);
        Task<CreateMediaUpload> UploadFiles(UploadModel uploadModel);
        Task<KeyValuePair<string, string>> CreateFolder(FolderModel uploadModel);

    }

    public class GoogleDriveService : IGoogleDriveService
    {
        //NOTE : only those folders and files accessible to api which are created by them
        private UserCredential _credential;
        private DriveService _driveService;
        private readonly string[] _scopes = { DriveService.Scope.DriveFile };



        public GoogleDriveService()
        {
            Initialize();
        }

        private void Initialize()
        {
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                _credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets, _scopes, "user",
                    CancellationToken.None, new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = "ShortVideo",
            });
        }

        public async Task<Dictionary<string, string>> GetFiles(int pageSize = 1000, string folderId = GoogleDriveFolderId.RootParentFolder)
        {
            return await GetFilesOrFolders($"'{folderId}' in parents and mimeType != 'application/vnd.google-apps.folder'", pageSize);
        }
        public async Task<Dictionary<string, string>> GetFolders(int pageSize = 1000, string folderId = GoogleDriveFolderId.RootParentFolder)
        {
            return await GetFilesOrFolders($"'{folderId}' in parents and mimeType = 'application/vnd.google-apps.folder'", pageSize);
        }
        public async Task<Dictionary<string, string>> GetFilesOrFolders(string query, int pageSize = 1000)
        {
            // Define parameters of request.
            var listRequest = _driveService.Files.List();
            listRequest.PageSize = pageSize;
            listRequest.Fields = "nextPageToken, files(id, name)";
            listRequest.Q = query;

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = (await listRequest.ExecuteAsync()).Files;

            var list = new Dictionary<string, string>();
            if (files != null && files.Count > 0)
                list = files.ToDictionary(a => a.Id, b => b.Name);
            return list;
        }

        public async Task<KeyValuePair<string, string>> GetFolderIdByName(string folderName)
        {
            var list = new Dictionary<string, string>();
            try
            {
                // Define parameters of request.
                var listRequest = _driveService.Files.List();
                listRequest.PageSize = 1000;
                listRequest.Fields = "nextPageToken, files(id, name)";
                listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder'";
                var folders = (await listRequest.ExecuteAsync()).Files;

                if (folders != null && folders.Count > 0)
                    list = folders.ToDictionary(a => a.Id, b => b.Name);

                return list.First(a => a.Value == null ? false : a.Value.Equals(folderName, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                return new KeyValuePair<string, string>(GlobalConstants.ErrorMessage, ex.Message);
            }
        }

        public async Task<CreateMediaUpload> UploadFiles(UploadModel uploadModel)
        {
            // uploading files
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = uploadModel.FileName,
                Parents = new List<string>() { uploadModel.ParentFolderId },
                Description = "",
                MimeType = uploadModel.ContentType,
            };

            if (!string.IsNullOrEmpty(uploadModel.ParentFolderId))
                fileMetadata.Parents = new List<string> { uploadModel.ParentFolderId };

            var fileExecute = _driveService.Files.Create(fileMetadata, new MemoryStream(uploadModel.ByteArray), uploadModel.ContentType);
            var res = await fileExecute.UploadAsync();
            fileExecute.ProgressChanged += (uploadProgress)
                 => RequestProgressChanged(uploadProgress, uploadModel.ByteArray.Length);

            return fileExecute;
        }

        public async Task<KeyValuePair<string, string>> CreateFolder(FolderModel folderModel)
        {
            var folder = await GetFolderIdByName(folderModel.FolderName);
            if (folder.Key != GlobalConstants.ErrorMessage)
                return folder;

            var file = _driveService.Files.Create(new Google.Apis.Drive.v3.Data.File() { Name = folderModel.FolderName, MimeType = folderModel.ContentType, Parents = new List<string> { folderModel.ParentFolderId } }).Execute();
            return new KeyValuePair<string, string>(file.Id, file.Name);
        }

        private void RequestProgressChanged(Google.Apis.Upload.IUploadProgress obj, long fileLength)
        {
            double pc = (obj.BytesSent * 100) / fileLength;
        }

        //https://developers.google.com/drive/api/v3/quickstart/dotnet
        //https://drive.google.com/drive/folders/197VtRLoVQn6_mwVfkyjHkqgBVBAqfaJb?usp=sharing
    }
}