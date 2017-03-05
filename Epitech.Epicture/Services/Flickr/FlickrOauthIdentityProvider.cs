using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Flickr;
using Epitech.Epicture.Model.Flickr.Core;
using Epitech.Epicture.Model.Imgur;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.Services.Flickr.Core;
using Epitech.Epicture.Services.Imgur.Core;
using Newtonsoft.Json.Linq;
using PCLCrypto;
using Plugin.Settings;

namespace Epitech.Epicture.Services.Flickr
{
    internal class FlickrOauthIdentityProvider : IOAuthIdentityProvider
    {
        public const string SecretKey = "bf6d7458ad3b06e4";
        private HttpClient _client;

        public string IdentityToken
        {
            get { return CrossSettings.Current.GetValueOrDefault<string>("FlickrIdentityToken"); }
            set { CrossSettings.Current.AddOrUpdateValue("FlickrIdentityToken", value); }
        }

        public string UserId
        {
            get { return CrossSettings.Current.GetValueOrDefault<string>("FlickrUserId"); }
            set { CrossSettings.Current.AddOrUpdateValue("FlickrUserId", value); }
        }

        public string UserName
        {
            get { return CrossSettings.Current.GetValueOrDefault<string>("FlickrUserName"); }
            set { CrossSettings.Current.AddOrUpdateValue("FlickrUserName", value); }
        }

        public bool NeedUserInput { get; }

        private string Frob { get; set; }

        public FlickrOauthIdentityProvider(bool needUserInput)
        {
            NeedUserInput = needUserInput;
        }

        public async Task<Uri> GetAuthorisationUrl()
        {
            Frob = null;
            try
            {
                var meth = $"?method=flickr.auth.getFrob&api_key={FlickrBaseClient.ClientId}&format=json&api_sig={Md5($"{SecretKey}api_key{FlickrBaseClient.ClientId}formatjsonmethodflickr.auth.getFrob")}";

                var response = new HttpRequestMessage(HttpMethod.Get, meth);

                var res = await Client.SendAsync(response);
                
                if (!res.IsSuccessStatusCode)
                    throw new Exception();

                var json = await res.Content.ReadAsStringAsync();
                json = json.Substring("jsonFlickrApi(".Length, json.Length - "jsonFlickrApi(".Length - 1);
                var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                var apiResponse = new FlickrApiResponse<JObject>
                {
                    Data = ((JObject)temp.First().Value).ToObject<JObject>(),
                    Success = (string)temp.First(pair => pair.Key == "stat").Value
                };

                Frob = apiResponse.Data.Value<string>("_content");

                var signature = Md5($"{SecretKey}api_key{FlickrBaseClient.ClientId}frob{Frob}permswrite");
                return new Uri($"http://www.flickr.com/services/auth/?api_key={FlickrBaseClient.ClientId}&perms=write&frob={Frob}&api_sig={signature}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public string GetAuthenticationHeader() => string.IsNullOrEmpty(IdentityToken) ? $"Client-ID {ImgurBaseClient.ClientId}" : $"Bearer {IdentityToken}";

        protected HttpClient Client => _client ?? (_client = new HttpClient
        {
            BaseAddress = new Uri("https://api.flickr.com/services/rest/")
        });

        public async Task Authorize(string pin)
        {
            try
            {
                var signature = Md5($"{SecretKey}api_key{FlickrBaseClient.ClientId}formatjsonfrob{Frob}methodflickr.auth.getToken");
                var meth = $"?method=flickr.auth.getToken&api_key={FlickrBaseClient.ClientId}&frob={Frob}&format=json&api_sig={signature}";

                var response = new HttpRequestMessage(HttpMethod.Get, meth);

                var res = await Client.SendAsync(response);

                if (!res.IsSuccessStatusCode)
                    throw new Exception();

                var json = await res.Content.ReadAsStringAsync();
                json = json.Substring("jsonFlickrApi(".Length, json.Length - "jsonFlickrApi(".Length - 1);
                var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                var apiResponse = new FlickrApiResponse<FlickrAuthorisation>
                {
                    Data = ((JObject)temp.First().Value).ToObject<FlickrAuthorisation>(),
                    Success = (string)temp.First(pair => pair.Key == "stat").Value
                };

                IdentityToken = apiResponse.Data.Token.Value;
                UserId = apiResponse.Data.AuthenticatedUser.Nsid;
                UserName = apiResponse.Data.AuthenticatedUser.Username;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task ReAuthorize()
        {
            throw new Exception("Unable to authenticate");
        }

        public static string Md5(string value)
        {
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Md5);
            var hash = hasher.HashData(Encoding.UTF8.GetBytes(value));
            var hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }
    }
}
