using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Epitech.Epicture.Services.Core
{
    internal class ImgurBaseClient
    {
        private static HttpClient _client;
        protected const string ClientId = "033ed1570301ce1";

        protected static HttpClient Client => _client ?? (_client = new HttpClient()
        {
            BaseAddress = new Uri("https://api.imgur.com/")
        });

        protected async Task<TReturn> Execute<T, TReturn>(HttpMethod method, string ressource, ImgurOauthIdentityProvider auth, Func<T, TReturn> selector)
        {
            try
            {
                var response = new HttpRequestMessage(method, ressource)
                {
                    Headers = {Authorization = AuthenticationHeaderValue.Parse($"{auth.GetAuthenticationHeader()}")}
                };

                return selector(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await (await Client.SendAsync(response)).Content.ReadAsStringAsync()));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return default(TReturn);
            }
        }
    }
}
