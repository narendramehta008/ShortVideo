using BaseLibrary.Constants;
using BaseLibrary.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortVideo.API.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShortVideo.API.Controllers
{
    public class FileManagerController : BaseController
    {
        private readonly IFileUploadService _uploadService;

        public FileManagerController(IHttpClientFactory clientFactory, IFileUploadService uploadService)
            : base(clientFactory)
        {
            new HttpService(_httpClient);
            _uploadService = uploadService;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> UploadImages([FromForm] IEnumerable<IFormFile> files)
        {
            return await _uploadService.UploadFiles(files, MediaConstants.SupportedFormats[MediaConstants.ImageFormats]);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadVideos([FromForm] IEnumerable<IFormFile> files)
        {
            return await _uploadService.UploadFiles(files, MediaConstants.SupportedFormats[MediaConstants.VideoFormats]);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFiles([FromForm] IEnumerable<IFormFile> files)
        {
            return await _uploadService.UploadFiles(files, MimeTypeMap.GetAllExtension());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadImagesToFolder([FromQuery] string folderName, [FromForm] IEnumerable<IFormFile> files)
        {
            return await _uploadService.UploadFilesByFolderName(files, MediaConstants.SupportedFormats[MediaConstants.ImageFormats], folderName);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadVideosToFolder([FromQuery] string folderName, [FromForm] IEnumerable<IFormFile> files)
        {
            return await _uploadService.UploadFilesByFolderName(files, MediaConstants.SupportedFormats[MediaConstants.VideoFormats], folderName);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFilesToFolder([FromQuery] string folderName, [FromForm] IEnumerable<IFormFile> files)
        {
            return await _uploadService.UploadFilesByFolderName(files, MimeTypeMap.GetAllExtension(), folderName);
        }
    }
}
