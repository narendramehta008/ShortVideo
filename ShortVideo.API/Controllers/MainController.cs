using Microsoft.AspNetCore.Mvc;
using ShortVideo.API.Models;
using ShortVideo.API.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShortVideo.API.Controllers
{
    public class MainController : BaseController
    {
        private readonly IGoogleDriveService _driveService;
        private readonly JoService _joService;

        public MainController(IHttpClientFactory clientFactory, IGoogleDriveService driveService)
            : base(clientFactory)
        {
            new HttpService(_httpClient);
            _driveService = driveService;
            _joService = new JoService(_httpClient);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> HashTagUrls()
        {
            return (await _joService.GetZeroResponse()).ResponseResult();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> UploadFiles()
        {
            //var localPath = @"E:\Files\Sample Videos";
            //var files = Directory.GetFiles(localPath).ToList();
            var videoUrls = await _joService.DownloadUrls();
            var serverFiles = await _driveService.GetFiles(folderId: "1GPU8UQNP7usViTXQInKOAGkXjrcHnw85");
            //var serverFiles = _driveService.GetFiles(1000, folderName: "ShortVideo");

            foreach (var item in videoUrls)
            {
                var name = item.Substring(item.LastIndexOf('/') + 1);
                var data = await (await _httpClient.GetAsync(item)).Content.ReadAsByteArrayAsync();

                //if (!files.Contains(name))
                //    System.IO.File.Create(@$"{localPath}\{name}").Write(data);

                if (serverFiles.Values.Contains(name))
                    continue;

                var uploadModel = new UploadModel()
                {
                    ByteArray = data,
                    ContentType = "video/mp4",
                    FileName = name
                };
                await _driveService.UploadFiles(uploadModel);
            }
            return Ok(videoUrls);
        }
    }
}