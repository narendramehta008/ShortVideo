using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

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
