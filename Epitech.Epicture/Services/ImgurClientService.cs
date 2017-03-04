using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Epitech.Epicture.Model;
using Epitech.Epicture.Model.Core;
using Epitech.Epicture.Services.Core;

namespace Epitech.Epicture.Services
{
    internal class ImgurClientService : ImgurBaseClient
    {
        public Task<List<ImgurGaleryAsset>> GetMainGalery(int page) => Execute<ImgurApiResponse<List<ImgurGaleryAsset>>, List<ImgurGaleryAsset>>(HttpMethod.Get, $"3/gallery/hot/viral/{page}", App.IdentityProvider, arg => arg.Data);
        public Task<List<ImgurGaleryAsset>> SearchMainGalery(string query, int page) => Execute<ImgurApiResponse<List<ImgurGaleryAsset>>, List<ImgurGaleryAsset>>(HttpMethod.Get, $"3/gallery/search/viral/{page}?q={Uri.EscapeDataString(query)}", App.IdentityProvider, arg => arg.Data);
        public Task<List<ImgurComment>> GetGalleryAssetComments(ImgurGaleryAsset asset) => Execute<ImgurApiResponse<List<ImgurComment>>, List<ImgurComment>>(HttpMethod.Get, $"3/gallery/image/{asset.Id}/comments", App.IdentityProvider, arg => arg.Data);
        public Task<string> FavoriteImage(ImgurGaleryAsset asset) => Execute<ImgurApiResponse<string>, string>(HttpMethod.Post, $"3/image/{asset.Id}/favorite", App.IdentityProvider, arg => arg.Data);
        public Task<ImgurGaleryAsset> GetImage(string assetId) => Execute<ImgurApiResponse<ImgurGaleryAsset>, ImgurGaleryAsset>(HttpMethod.Get, $"3/image/{assetId}", App.IdentityProvider, arg => arg.Data);

        public async Task UploadImage(Stream image)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "3/image")
                {
                    Headers = {Authorization = AuthenticationHeaderValue.Parse($"{App.IdentityProvider.GetAuthenticationHeader()}")},
                    Content = new StreamContent(image)
                };

                var response = await Client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.Unauthorized) {
                    await App.IdentityProvider.ReAuthorize();
                }
                if (!response.IsSuccessStatusCode)
                    throw new Exception();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw new Exception("Unable to upload this image");
            }
        }
    }
}
