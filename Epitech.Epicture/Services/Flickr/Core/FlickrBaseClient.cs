using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Flickr.Core;
using Epitech.Epicture.Services.Contracts;
using Newtonsoft.Json.Linq;

namespace Epitech.Epicture.Services.Flickr.Core
{
    public class FlickrBaseClient : IBaseClient
    {
        private HttpClient _client;
        internal const string ClientId = "370021b537ba69d7cae0c26ba4565ecc";

        protected HttpClient Client => _client ?? (_client = new HttpClient
        {
            BaseAddress = new Uri("https://api.flickr.com/services/rest/")
        });

        public IOAuthIdentityProvider IdentityProvider { get; } = new FlickrOauthIdentityProvider(false);

        public static string ServiceName => "Flickr";

        protected async Task<TReturn> Execute<T, TReturn>(HttpMethod method, string logicMethod, Dictionary<string, string> param, string mappingName, Func<FlickrApiResponse<T>, TReturn> selector)
        {
            try
            {
                var response = new HttpRequestMessage(method, GetMethodDescription(logicMethod, param));

                var res = await Client.SendAsync(response);

                if (res.StatusCode == HttpStatusCode.Unauthorized)
                    await IdentityProvider.ReAuthorize();
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

        protected async Task<TReturn> Execute<T, TReturn>(HttpMethod method, string logicMethod, Dictionary<string, string> param, string mappingName,
            Func<FlickrApiResponse<T>, Task<TReturn>> selector)
        {
            try
            {
                var response = new HttpRequestMessage(method, GetMethodDescription(logicMethod, param));

                var res = await Client.SendAsync(response);

                if (res.StatusCode == HttpStatusCode.Unauthorized)
                    await IdentityProvider.ReAuthorize();
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

        protected async Task<TReturn> ExecuteNoReturn<TReturn>(HttpMethod method, string logicMethod, Dictionary<string, string> param, string mappingName, Func<FlickrApiResponse<object>, TReturn> selector)
        {
            try
            {
                var response = new HttpRequestMessage(method, GetMethodDescription(logicMethod, param));

                var res = await Client.SendAsync(response);

                if (res.StatusCode == HttpStatusCode.Unauthorized)
                    await IdentityProvider.ReAuthorize();
                if (!res.IsSuccessStatusCode)
                    throw new Exception();

                var json = await res.Content.ReadAsStringAsync();
                json = json.Substring("jsonFlickrApi(".Length, json.Length - "jsonFlickrApi(".Length - 1);
                var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                var apiResponse = new FlickrApiResponse<object>
                {
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

        protected string GetMethodDescription(string method, Dictionary<string, string> param)
        {
            var def = new Dictionary<string, string>()
            {
                { "method", method },
                { "api_key", ClientId },
                { "format", "json" }
            };
            if (!string.IsNullOrEmpty(IdentityProvider.IdentityToken))
                def.Add("auth_token", IdentityProvider.IdentityToken);

            var meth = def.Aggregate("", (current, s) => current + $"&{s.Key}={s.Value}");
            meth = param.Aggregate(meth, (current, s) => current + $"&{s.Key}={s.Value}");
            meth = "?" + meth.Substring(1);

            if (string.IsNullOrEmpty(IdentityProvider.IdentityToken))
                return meth;
            var hashSource = def.Select(s => s.Key + s.Value).ToList();
            hashSource.AddRange(param.Select(s => s.Key + s.Value).ToList());
            hashSource.Sort();

            var hashSourceString = hashSource.Aggregate(FlickrOauthIdentityProvider.SecretKey, (current, s) => current + s);
            meth += $"&api_sig={FlickrOauthIdentityProvider.Md5(hashSourceString)}";

            return meth;
        }
    }
}
