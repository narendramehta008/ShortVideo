using BaseLibrary.Constants;
using BaseLibrary.Helpers;
using BaseLibrary.Models.APIResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortVideo.API.Models;
using ShortVideo.API.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShortVideo.API.Services
{
    public interface IFileUploadService
    {
        Task<IActionResult> UploadFiles(FileQueryModel queryModel, IEnumerable<IFormFile> files);

        Task<IActionResult> UploadFilesByFolderId(IEnumerable<IFormFile> files, IEnumerable<string> validFormats, string folderId);

        Task<IActionResult> UploadFilesByFolderName(FileQueryModel queryModel, IEnumerable<IFormFile> files);

        //Task<Dictionary<string, FileResult>> GetFilesByFolderName(string folderName, [CallerMemberName] string caller = null);
        Task<Dictionary<string, List<string>>> GetViewLinkByFolderName(FileQueryModel queryModel);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IGoogleDriveService _driveService;

        public FileUploadService(IGoogleDriveService driveService)
        {
            _driveService = driveService;
        }

        /// <summary>
        /// upload files to default folder according to file types
        /// </summary>
        /// <param name="files"></param>
        /// <param name="validFormats"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public async Task<IActionResult> UploadFiles(FileQueryModel queryModel, IEnumerable<IFormFile> files)
        {
            if (files.Count() == 0)
                return new BadRequestResult();
            string folderId;
            IEnumerable<string> validFormats = MediaConstants.SupportedFormats[queryModel.FileType];
            switch (queryModel.FileType)
            {
                case FileType.Image:
                    folderId = GoogleDriveFolderId.DefaultImageFolder;
                    break;

                case FileType.Video:
                    folderId = GoogleDriveFolderId.DefaultVideoFolder;
                    break;

                case FileType.File:
                    folderId = GoogleDriveFolderId.DefaultFileFolder;
                    break;

                default:
                    return new BadRequestResult();
            }
            return await UploadFilesByFolderId(files, validFormats, folderId);
        }

        public async Task<IActionResult> UploadFilesByFolderName(FileQueryModel queryModel, IEnumerable<IFormFile> files)
        {
            var folders = queryModel.FolderName.Split(',').ToList();
            var folder = await _driveService.GetFolderIdByName(folders.Last());
            IEnumerable<string> validFormats = MediaConstants.SupportedFormats[queryModel.FileType];

            if (folder.Key == GlobalConstants.ErrorMessage)
            {
                switch (queryModel.FileType)
                {
                    case FileType.Image:
                        folder = await GetFolderId(queryModel, GoogleDriveFolderId.DefaultImageParentFolder);
                        break;

                    case FileType.Video:
                        folder = await GetFolderId(queryModel, GoogleDriveFolderId.DefaultVideoParentFolder);
                        break;

                    case FileType.File:
                        folder = await GetFolderId(queryModel, GoogleDriveFolderId.DefaultFileParentFolder);
                        break;
                }
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

        /// <summary>
        /// It checks folders from all the type directories(file, image, video)
        /// NOTE : Here web links is generated only for shared folders and we given viewers permissions manually to avoid api abusing
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<string>>> GetViewLinkByFolderName(FileQueryModel queryModel)
        {
            var folder = await _driveService.GetFolderIdByName(queryModel.FolderName);
            return await _driveService.GetFilesViewLinks(folder);
        }

        private async Task<KeyValuePair<string, string>> GetFolderId(FileQueryModel queryModel, string parentFolder)
        {
            var existingFolders = await _driveService.GetAllFolders();
            var folders = queryModel.FolderName.Split(',').ToArray();
            var folder = new KeyValuePair<string, string>();
            var tempFolder = existingFolders.FirstOrDefault(a => a.Value.Equals(folders[0], StringComparison.OrdinalIgnoreCase));

            for (int index = 0; index < folders.Length; index++)
            {
                if (index == 0 && string.IsNullOrWhiteSpace(tempFolder.Value))
                    folder = await _driveService.CreateFolder(new FolderModel { FolderName = queryModel.FolderName, ParentFolderId = parentFolder });
                else if (index == 0)
                    folder = new KeyValuePair<string, string>(tempFolder.Key, tempFolder.Value);
                else if (index > 0)
                {
                    tempFolder = existingFolders.FirstOrDefault(a => a.Value.Equals(folders[index], StringComparison.OrdinalIgnoreCase));
                    if (string.IsNullOrWhiteSpace(tempFolder.Value))
                        folder = await _driveService.CreateFolder(new FolderModel { FolderName = folders[index], ParentFolderId = folder.Key });
                    else
                        folder = new KeyValuePair<string, string>(tempFolder.Key, tempFolder.Value);
                }
            }

            return folder;
        }

        //public async Task<Dictionary<string, FileResult>> GetFilesByFolderName(FileQueryModel queryModel)
        //{
        //    string folderId;
        //    switch (queryModel.FileType)
        //    {
        //        case FileType.Image:
        //            folder = await _driveService.CreateFolder(new FolderModel { FolderName = queryModel.FolderName, ParentFolderId = GoogleDriveFolderId.DefaultVideoParentFolder });
        //            validFormats = MediaConstants.SupportedFormats[MediaConstants.ImageFormats];
        //            break;
        //        case FileType.Video:
        //            folder = await _driveService.CreateFolder(new FolderModel { FolderName = queryModel.FolderName, ParentFolderId = GoogleDriveFolderId.DefaultVideoParentFolder });
        //            validFormats = MediaConstants.SupportedFormats[MediaConstants.VideoFormats];
        //            break;
        //        case FileType.File:
        //            folder = await _driveService.CreateFolder(new FolderModel { FolderName = queryModel.FolderName, ParentFolderId = GoogleDriveFolderId.DefaultFileParentFolder });
        //            validFormats = MimeTypeMap.GetAllExtension();
        //            break;
        //        default:
        //            return new BadRequestResult();
        //    }

        //    return await _driveService.GetFilesStream(folderId: folderId);
        //}
    }
}