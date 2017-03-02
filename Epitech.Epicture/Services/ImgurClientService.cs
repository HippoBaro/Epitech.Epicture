using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Epitech.Epicture.Model;
using Epitech.Epicture.Model.Core;

namespace Epitech.Epicture.Services
{
    internal class ImgurClientService
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
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurApiResponse<List<ImgurGaleryAsset>>>(response).Data;
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
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurApiResponse<List<ImgurGaleryAsset>>>(response).Data;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task<List<ImgurComment>> GetGalleryAssetComments(ImgurGaleryAsset asset)
        {
            try
            {
                var response = await Client.GetStringAsync($"gallery/image/{asset.Id}/comments");
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurApiResponse<List<ImgurComment>>>(response).Data;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
