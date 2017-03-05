using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Epitech.Epicture.Model;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.Services.Imgur.Core;
using Plugin.Settings;

namespace Epitech.Epicture.Services.Imgur
{
    internal class ImgurOauthIdentityProvider : IOAuthIdentityProvider
    {
        private const string SecretKey = "fd65e9b0baf0b8ca3dae5d6916942ebf54e04a99";
        private HttpClient _client;

        public string IdentityToken
        {
            get { return CrossSettings.Current.GetValueOrDefault<string>("IdentityToken"); }
            set { CrossSettings.Current.AddOrUpdateValue("IdentityToken", value); }
        }

        public string RefreshToken
        {
            get { return CrossSettings.Current.GetValueOrDefault<string>("RefreshToken"); }
            set { CrossSettings.Current.AddOrUpdateValue("RefreshToken", value); }
        }

        public string UserId
        {
            get { return CrossSettings.Current.GetValueOrDefault<string>("UserId"); }
            set { CrossSettings.Current.AddOrUpdateValue("UserId", value); }
        }

        public string UserName
        {
            get { return CrossSettings.Current.GetValueOrDefault<string>("UserName"); }
            set { CrossSettings.Current.AddOrUpdateValue("UserName", value); }
        }

        public Uri GetAuthorisationUrl() => new Uri($"https://api.imgur.com/oauth2/authorize?client_id={ImgurBaseClient.ClientId}&response_type=pin&state=authorizeXamForms");

        public string GetAuthenticationHeader() => string.IsNullOrEmpty(IdentityToken) ? $"Client-ID {ImgurBaseClient.ClientId}" : $"Bearer {IdentityToken}";

        protected HttpClient Client => _client ?? (_client = new HttpClient
        {
            BaseAddress = new Uri("https://api.imgur.com/")
        });

        public async Task Authorize(string pin)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "oauth2/token");

                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "pin"),
                    new KeyValuePair<string, string>("client_secret", SecretKey),
                    new KeyValuePair<string, string>("client_id", ImgurBaseClient.ClientId),
                    new KeyValuePair<string, string>("pin", pin)
                };
                request.Content = new FormUrlEncodedContent(keyValues);

                var response = await Client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Unable to authenticate");
                var auth = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurAuthorisation>(await response.Content.ReadAsStringAsync());
                IdentityToken = auth.AccessToken;
                UserId = auth.AccountId;
                UserName = auth.AccountUsername;
                RefreshToken = auth.RefreshToken;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw new Exception("Unable to authenticate");
            }
        }

        public async Task ReAuthorize()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "oauth2/token");

                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("refresh_token", RefreshToken),
                    new KeyValuePair<string, string>("client_secret", SecretKey),
                    new KeyValuePair<string, string>("client_id", ImgurBaseClient.ClientId),
                    new KeyValuePair<string, string>("grant_type", "refresh_token")
                };
                request.Content = new FormUrlEncodedContent(keyValues);

                var response = await Client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Unable to authenticate");
                var auth = Newtonsoft.Json.JsonConvert.DeserializeObject<ImgurAuthorisation>(await response.Content.ReadAsStringAsync());
                IdentityToken = auth.AccessToken;
                UserName = auth.AccountUsername;
                RefreshToken = auth.RefreshToken;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                IdentityToken = null;
                UserName = null;
                RefreshToken = null;
                throw new Exception("Unable to authenticate");
            }
        }
    }
}
