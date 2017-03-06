using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Epitech.Epicture.Services.Contracts;

namespace Epitech.Epicture.Services.Imgur.Core
{
    public class ImgurBaseClient : IBaseClient
    {
        private HttpClient _client;
        internal const string ClientId = "033ed1570301ce1";

        protected HttpClient Client => _client ?? (_client = new HttpClient
        {
            BaseAddress = new Uri("https://api.imgur.com/")
        });

        public IOAuthIdentityProvider IdentityProvider { get; } = new ImgurOauthIdentityProvider(true);

        public static string ServiceName => "Imgur";

        protected async Task<TReturn> Execute<T, TReturn>(HttpMethod method, string ressource, Func<T, TReturn> selector)
        {
            try
            {
                var response = new HttpRequestMessage(method, ressource)
                {
                    Headers = {Authorization = AuthenticationHeaderValue.Parse($"{IdentityProvider.GetAuthenticationHeader()}")}
                };

                var res = await Client.SendAsync(response);

                if (res.StatusCode == HttpStatusCode.Unauthorized)
                    await IdentityProvider.ReAuthorize();
                if (!res.IsSuccessStatusCode)
                    throw new Exception();

                return selector(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await res.Content.ReadAsStringAsync()));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return default(TReturn);
            }
        }
    }
}
