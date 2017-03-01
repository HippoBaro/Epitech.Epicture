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

        public async Task<List<ImgurGaleryAsset>> GetMainGalery()
        {
            try
            {
                var response = await Client.GetStringAsync("gallery/hot/viral/0");
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
