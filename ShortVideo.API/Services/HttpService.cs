using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShortVideo.API.Services
{
    public interface IHttpService
    {
        void SetHeaders();
    }
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        public HttpService(HttpClient httpClient, bool isSetDefaultHeaders = true)
        {
            _httpClient = httpClient;
            if (isSetDefaultHeaders)
                SetDefaultHeader();
        }

        private void SetDefaultHeader()
        {
            //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=UTF-8");
            _httpClient.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //_httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");

            _httpClient.DefaultRequestHeaders.Add("history-policy", "2");
            _httpClient.DefaultRequestHeaders.Add("X-Request-Id", "91e0f5cc-59f5-4659-a908-9f79919b942e_1600574093824");
            _httpClient.DefaultRequestHeaders.Add("uc", "w");
            _httpClient.DefaultRequestHeaders.Add("ucq", "FAST");

            _httpClient.DefaultRequestHeaders.Add("user-uuid", "");
            _httpClient.DefaultRequestHeaders.Add("auth-token", "c95e9032- | 1600573623280 | user");
            _httpClient.DefaultRequestHeaders.Add("install-referrer", "");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 5.1.1; SM-G965N Build/QP1A.190711.020)");
            _httpClient.DefaultRequestHeaders.Accept.
                Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public void SetHeaders()
        {

        }
    }
}
