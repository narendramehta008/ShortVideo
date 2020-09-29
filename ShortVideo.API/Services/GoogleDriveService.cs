using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using System.Collections.Generic;
using static Google.Apis.Drive.v3.FilesResource;
using System.Linq;
using ShortVideo.API.Models;
using System.Threading.Tasks;

namespace ShortVideo.API.Services
{
    public interface IGoogleDriveService
    {
        Dictionary<string, string> GetFiles(int pageSize = 100, string folderName = null);
        Task<CreateMediaUpload> UploadFiles(UploadModel uploadModel);
    }

    public class GoogleDriveService : IGoogleDriveService
    {
        private UserCredential _credential;
        private DriveService _driveService;
        readonly string[] _scopes = { DriveService.Scope.DriveFile };

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
                string credPath = "tokens.json";
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
        public Dictionary<string, string> GetFiles(int pageSize = 100, string folderName = null)
        {
            // Define parameters of request.
            var listRequest = _driveService.Files.List();
            listRequest.PageSize = pageSize;
            listRequest.Fields = "nextPageToken, files(id, name)";

            if (!string.IsNullOrWhiteSpace(folderName))
                listRequest.Q = $"'{folderName}' in parents";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

            var list = new Dictionary<string, string>();
            if (files != null && files.Count > 0)
                list = files.ToDictionary(a => a.Id, b => b.Name);
            return list;
        }

        public async Task<CreateMediaUpload> UploadFiles(UploadModel uploadModel)
        {
            // uploading files
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = uploadModel.FileName,
                Parents = new List<string>() { uploadModel.FolderId },
                Description = "",
                MimeType = uploadModel.ContentType,

            };

            if (!string.IsNullOrEmpty(uploadModel.FolderId))
                fileMetadata.Parents = new List<string> { uploadModel.FolderId };

            var fileExecute = _driveService.Files.Create(fileMetadata, new MemoryStream(uploadModel.ByteArray), uploadModel.ContentType);
            var res = await fileExecute.UploadAsync();
            fileExecute.ProgressChanged += (uploadProgress)
                 => RequestProgressChanged(uploadProgress, uploadModel.ByteArray.Length);

            return fileExecute;
        }
        private void RequestProgressChanged(Google.Apis.Upload.IUploadProgress obj, long fileLength)
        {
            double pc = (obj.BytesSent * 100) / fileLength;

        }

        //https://developers.google.com/drive/api/v3/quickstart/dotnet
        //https://drive.google.com/drive/folders/197VtRLoVQn6_mwVfkyjHkqgBVBAqfaJb?usp=sharing
    }



}
