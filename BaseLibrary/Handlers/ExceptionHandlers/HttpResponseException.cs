using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace BaseLibrary.Handlers.ExceptionHandlers
{
    public class HttpResponseException : Exception
    {
        public HttpResponseException(IEnumerable<string> messages, HttpStatusCode httpStatus = HttpStatusCode.NotFound)
            : base(JsonConvert.SerializeObject(messages))
        {
            Messages = messages;
            StatusCode = httpStatus;
        }

        public HttpResponseException(string message, HttpStatusCode httpStatus = HttpStatusCode.NotFound)
            : base(message)
        {
            Messages = new[] { message };
            StatusCode = httpStatus;
        }

        public IEnumerable<string> Messages { get; }
        public HttpStatusCode StatusCode { get; }
    }
}