using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ShortVideo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly HttpClient _httpClient;

        public BaseController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
        }
    }
}