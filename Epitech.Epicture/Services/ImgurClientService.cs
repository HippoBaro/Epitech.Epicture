using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Epitech.Epicture.Model;

namespace Epitech.Epicture.Services
{
    public class ImgurClientService
    {
        private HttpClient _client;
        private const string ClientId = "e69b45bb75887dd";

        public HttpClient Client => _client ?? (_client = new HttpClient()
        {
            BaseAddress = new Uri("https://api.imgur.com/3/"),
            DefaultRequestHeaders = {Authorization = AuthenticationHeaderValue.Parse($"Client-ID {ClientId}")}
        });

        public async Task<List<ImgurGaleryAsset>> GetMainGalery(int page)
        {
            try
            {
                var response = await Client.GetStringAsync($"gallery/hot/viral/{page}");
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurApiResponse<ImgurGaleryAsset>>(response).Data;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task<List<ImgurGaleryAsset>> SearchMainGalery(string query, int page)
        {
            try
            {
                var response = await Client.GetStringAsync($"gallery/search/viral/{page}?q={Uri.EscapeDataString(query)}");
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurApiResponse<ImgurGaleryAsset>>(response).Data;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
