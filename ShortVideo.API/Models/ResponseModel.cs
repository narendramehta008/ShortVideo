using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace ShortVideo.API.Models
{
    public class ResponseModel
    {
        //public ResponseModel([ActionResultStatusCode] int statusCode)
        public ResponseModel(HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }

        public HttpStatusCode StatusCode { get; } = HttpStatusCode.OK;
        public IEnumerable<string> Errors { get; set; }
        public object Data { get; set; }
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        public IActionResult ResponseResult()
        {
            if (IsSuccess)
                return new OkObjectResult(this);
            else
                return new BadRequestObjectResult(Data);
        }
    }
}