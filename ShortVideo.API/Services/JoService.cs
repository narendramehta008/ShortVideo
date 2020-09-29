using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ShortVideo.API.Models;

namespace ShortVideo.API.Services
{
    public class JoService
    {
        private readonly HttpClient _httpClient;

        public JoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseModel> GetZeroResponse()
        {
            var postData = "{\"android_id\":\"5f1a3c7f8bd9f04e\",\"appOpenEvent\":false,\"client_info\":{\"app_language\":\"en\",\"app_version\":\"2.1.11\",\"brand\":\"Josh\",\"default_notification_lang\":\"en\",\"device\":\"android\",\"gaid\":\"\",\"gaid_opt_out_status\":false,\"height\":1280,\"manufacturer\":\"samsung\",\"model\":\"SM-G965N\",\"os_version\":\"5.1.1\",\"width\":720,\"android_id\":\"5f1a3c7f8bd9f04e\",\"client_id\":\"91e0f5cc-59f5-4659-a908-9f79919b942e\",\"udid\":\"5f1a3c7f8bd9f04e\"},\"connection_info\":{\"apn_name\":\"\\\"WiredSSID\\\"\",\"cellid\":\"\",\"connection\":\"w\"},\"handshake_version\":\"438\",\"install_type\":\"NA\",\"location_info\":{\"is_GPS_location\":false,\"lat\":\"\",\"lon\":\"\"},\"packageName\":\"com.eterno.shortvideos\",\"referrer\":\"utm_source=google-play&utm_medium=organic\"}";
            var httpResponse = await _httpClient.PostAsync("http://gateway.coolfie.io/api/v1/handshake/",
                new StringContent(postData, Encoding.UTF8, "application/json"));

            var textResponse = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
                textResponse = "{\"data\":{\"zero_search\":\"zero_search\"}}";

            var response = new ResponseModel(httpResponse.StatusCode)
            {
                Data = JObject.Parse(textResponse)["data"]["zero_search"]
            };
            return response;
        }

        public async Task<IEnumerable<string>> DownloadUrls()
        {
            var downloadUrls = new List<string>();
            var httpResponse = await _httpClient.GetAsync("https://feed.coolfie.io/feed/latest?page=0&rows=100");
            if (!httpResponse.IsSuccessStatusCode)
                return downloadUrls;

            var parsedData = JObject.Parse(await httpResponse.Content.ReadAsStringAsync())["data"];
            foreach (var item in parsedData)
            {
                downloadUrls.Add(item["download_url"].ToString());
            }
            //add distinct
            return downloadUrls;
        }
    }

    //https://share.myjosh.in/profile/bf758d0c-114e-472e-9156-894219692607  profile
    //https://feed.coolfie.io/user/feed/latest/bf758d0c-114e-472e-9156-894219692607?page=7&rows=10 user profile videos
    //"https://feed.coolfie.io/user/feed/trending/bf758d0c-114e-472e-9156-894219692607/?page=0&rows=10"

}
