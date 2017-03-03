using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Epitech.Epicture.Services.Core
{
    internal class ImgurBaseClient
    {
        private HttpClient _client;
        protected const string ClientId = "033ed1570301ce1";

        protected HttpClient Client => _client ?? (_client = new HttpClient()
        {
            BaseAddress = new Uri("https://api.imgur.com/"),
            DefaultRequestHeaders = { Authorization = AuthenticationHeaderValue.Parse($"Client-ID {ClientId}") }
        });

        protected async Task<TReturn> GetRessource<T, TReturn>(string ressource, Func<T, TReturn> selector)
        {
            try
            {
                var response = await Client.GetStringAsync(ressource);
                return selector(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return default(TReturn);
            }
        }
    }
}
