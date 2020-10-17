using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShortVideo.API.Models.QueryModels;
using ShortVideo.API.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShortVideo.API.Controllers
{
    public class FileManagerController : BaseController
    {
        private readonly IFileUploadService _uploadService;
        private readonly IMemoryCache _memoryCache;

        public FileManagerController(IHttpClientFactory clientFactory, IFileUploadService uploadService, IMemoryCache memoryCache)
            : base(clientFactory)
        {
            new HttpService(_httpClient);
            _uploadService = uploadService;
            _memoryCache = memoryCache;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFiles([FromQuery] FileQueryModel queryModel, [FromForm] IEnumerable<IFormFile> files)
        {
            return await _uploadService.UploadFiles(queryModel, files);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFilesToFolder([FromQuery] FileQueryModel queryModel, [FromForm] IEnumerable<IFormFile> files)
        {
            return await _uploadService.UploadFilesByFolderName(queryModel, files);
        }

        /// <summary>
        /// adding temporary cache to avoid the hitting multiple times for same request
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<Dictionary<string, List<string>>> GetFilesViewLink([FromQuery] FileQueryModel queryModel)
        {
            bool isExist = _memoryCache.TryGetValue(queryModel.FolderName, out Dictionary<string, List<string>> viewsByFolderName);
            if (!isExist)
            {
                viewsByFolderName = await _uploadService.GetViewLinkByFolderName(queryModel);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(queryModel.FolderName, viewsByFolderName, cacheEntryOptions);
            }
            return viewsByFolderName;
        }

        //[HttpGet("[action]")]
        //public async Task<Dictionary<string, FileResult>> GetFilesFromFolder([FromQuery] string folderName)
        //{
        //    return await _uploadService.GetFilesFromFolder(folderName);
        //}
    }
}