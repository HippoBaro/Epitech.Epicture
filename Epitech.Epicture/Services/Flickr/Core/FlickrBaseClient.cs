using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Flickr.Core;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.Services.Imgur;
using Newtonsoft.Json.Linq;

namespace Epitech.Epicture.Services.Flickr.Core
{
    internal class FlickrBaseClient : IBaseClient
    {
        private HttpClient _client;
        public const string ClientId = "370021b537ba69d7cae0c26ba4565ecc";

        protected HttpClient Client => _client ?? (_client = new HttpClient
        {
            BaseAddress = new Uri("https://api.flickr.com/services/rest/")
        });

        public IOAuthIdentityProvider IdentityProvider { get; } = new ImgurOauthIdentityProvider();

        protected async Task<TReturn> Execute<T, TReturn>(HttpMethod method, string logicMethod, Dictionary<string, string> param, Func<FlickrApiResponse<T>, TReturn> selector)
        {
            try
            {
                var meth = $"?method={logicMethod}&api_key={ClientId}&format=json";
                foreach (var s in param)
                    meth += $"&{s.Key}={s.Value}";

                var response = new HttpRequestMessage(method, meth)
                {
                    //Headers = {Authorization = AuthenticationHeaderValue.Parse($"{IdentityProvider.GetAuthenticationHeader()}")}
                };

                var res = await Client.SendAsync(response);

                //if (res.StatusCode == HttpStatusCode.Unauthorized)
                //    await IdentityProvider.ReAuthorize();
                if (!res.IsSuccessStatusCode)
                    throw new Exception();

                var json = await res.Content.ReadAsStringAsync();
                json = json.Substring("jsonFlickrApi(".Length, json.Length - "jsonFlickrApi(".Length - 1);
                var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                var apiResponse = new FlickrApiResponse<T>
                {
                    Data = ((JObject)temp.First().Value).ToObject<T>(),
                    Success = (string)temp.First(pair => pair.Key == "stat").Value
                };

                return selector(apiResponse);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return default(TReturn);
            }
        }

        protected async Task<TReturn> Execute<T, TReturn>(HttpMethod method, string logicMethod, Dictionary<string, string> param, Func<FlickrApiResponse<T>, Task<TReturn>> selector)
        {
            try
            {
                var meth = $"?method={logicMethod}&api_key={ClientId}&format=json";
                foreach (var s in param)
                    meth += $"&{s.Key}={s.Value}";

                var response = new HttpRequestMessage(method, meth)
                {
                    //Headers = {Authorization = AuthenticationHeaderValue.Parse($"{IdentityProvider.GetAuthenticationHeader()}")}
                };

                var res = await Client.SendAsync(response);

                //if (res.StatusCode == HttpStatusCode.Unauthorized)
                //    await IdentityProvider.ReAuthorize();
                if (!res.IsSuccessStatusCode)
                    throw new Exception();

                var json = await res.Content.ReadAsStringAsync();
                json = json.Substring("jsonFlickrApi(".Length, json.Length - "jsonFlickrApi(".Length - 1);
                var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                var apiResponse = new FlickrApiResponse<T>
                {
                    Data = ((JObject) temp.First().Value).ToObject<T>(),
                    Success = (string) temp.First(pair => pair.Key == "stat").Value
                };

                return await selector(apiResponse);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return default(TReturn);
            }
        }
    }
}
