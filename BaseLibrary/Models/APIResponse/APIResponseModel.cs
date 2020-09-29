using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace BaseLibrary.Models.APIResponse
{
    public class APIResponseModel
    {
        public APIResponseModel(HttpStatusCode httpStatusCode)
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