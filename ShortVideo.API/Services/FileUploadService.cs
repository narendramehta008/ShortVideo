using BaseLibrary.Constants;
using BaseLibrary.Helpers;
using BaseLibrary.Models.APIResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortVideo.API.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ShortVideo.API.Services
{
    public interface IFileUploadService
    {
        Task<IActionResult> UploadFiles(IEnumerable<IFormFile> files, IEnumerable<string> validFormats, [CallerMemberName] string caller = null);

        Task<IActionResult> UploadFilesByFolderId(IEnumerable<IFormFile> files, IEnumerable<string> validFormats, string folderId);

        Task<IActionResult> UploadFilesByFolderName(IEnumerable<IFormFile> files, IEnumerable<string> validFormats, string folderName, [CallerMemberName] string caller = null);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly IGoogleDriveService _driveService;

        public FileUploadService(IHttpClientFactory clientFactory, IGoogleDriveService driveService)
        {
            _httpClient = clientFactory.CreateClient();
            _driveService = driveService;
        }
        /// <summary>
        /// upload files to default folder according to file types
        /// </summary>
        /// <param name="files"></param>
        /// <param name="validFormats"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task<IActionResult> UploadFiles(IEnumerable<IFormFile> files, IEnumerable<string> validFormats, [CallerMemberName] string caller = "")
        {
            if (files.Count() == 0)
                return new BadRequestResult();
            string folderId;
            if (caller.Contains("image", System.StringComparison.OrdinalIgnoreCase))
                folderId = GoogleDriveFolderId.DefaultImageFolder;
            else if (caller.Contains("video", System.StringComparison.OrdinalIgnoreCase))
                folderId = GoogleDriveFolderId.DefaultVideoFolder;
            else
                folderId = GoogleDriveFolderId.DefaultFileFolder;

            return await UploadFilesByFolderId(files, validFormats, folderId);
        }

        public async Task<IActionResult> UploadFilesByFolderName(IEnumerable<IFormFile> files, IEnumerable<string> validFormats, string folderName, [CallerMemberName] string caller = null)
        {
            var folder = await _driveService.GetFolderIdByName(folderName);
            if (folder.Key == GlobalConstants.ErrorMessage)
            {
                if (caller.Contains("image", System.StringComparison.OrdinalIgnoreCase))
                    folder = await _driveService.CreateFolder(new FolderModel { FolderName = folderName, ParentFolderId = GoogleDriveFolderId.DefaultVideoParentFolder });
                else if (caller.Contains("video", System.StringComparison.OrdinalIgnoreCase))
                    folder = await _driveService.CreateFolder(new FolderModel { FolderName = folderName, ParentFolderId = GoogleDriveFolderId.DefaultVideoParentFolder });
                else
                    folder = await _driveService.CreateFolder(new FolderModel { FolderName = folderName, ParentFolderId = GoogleDriveFolderId.DefaultFileParentFolder });
            }

            return await UploadFilesByFolderId(files, validFormats, folder.Key);
        }

        public async Task<IActionResult> UploadFilesByFolderId(IEnumerable<IFormFile> files, IEnumerable<string> validFormats, string folderId)
        {
            var serverFiles = await _driveService.GetFiles(folderId: folderId);

            var response = new Dictionary<string, StatusModel>();
            foreach (var item in files)
            {
                var name = item.FileName.Substring(item.FileName.LastIndexOf('/') + 1);
                var ext = Utils.GetFileExtension(item.FileName);
                if (!validFormats.Contains(ext))
                {
                    response.Add(name, new StatusModel("Not a supported extension."));
                    continue;
                }

                var memory = new MemoryStream();
                await item.OpenReadStream().CopyToAsync(memory);

                //if (!files.Contains(name))
                //    System.IO.File.Create(@$"{localPath}\{name}").Write(data);

                if (serverFiles.Values.Contains(name))
                {
                    response.Add(name, new StatusModel("File already exists."));
                    continue;
                }

                var uploadModel = new UploadModel()
                {
                    ParentFolderId = folderId,
                    ByteArray = memory.ToArray(),
                    ContentType = MimeTypeMap.GetMimeType(ext),
                    FileName = name
                };
                await _driveService.UploadFiles(uploadModel);

                response.Add(name, new StatusModel());
            }
            return new APIResponseModel(System.Net.HttpStatusCode.OK) { Data = response }.ResponseResult();
        }
    }
}