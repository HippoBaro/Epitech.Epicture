using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Model.Imgur;
using Epitech.Epicture.Model.Imgur.Core;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.Services.Imgur.Core;

namespace Epitech.Epicture.Services.Imgur
{
    internal class ImgurClientService : ImgurBaseClient, IImageClientService
    {
        public Task<List<IImageAsset>> GetMainGalery(int page) => Execute<ImgurApiResponse<List<ImgurGaleryAsset>>, List<IImageAsset>>(HttpMethod.Get, $"3/gallery/hot/viral/{page}", arg => new List<IImageAsset>(arg.Data));
        public Task<List<IImageAsset>> SearchMainGalery(string query, int page) => Execute<ImgurApiResponse<List<ImgurGaleryAsset>>, List<IImageAsset>>(HttpMethod.Get, $"3/gallery/search/viral/{page}?q={Uri.EscapeDataString(query)}", arg => new List<IImageAsset>(arg.Data));
        public Task<List<IAssetComment>> GetGalleryAssetComments(IImageAsset asset) => Execute<ImgurApiResponse<List<ImgurComment>>, List<IAssetComment>>(HttpMethod.Get, $"3/gallery/image/{asset.Id}/comments", arg => new List<IAssetComment>(arg.Data));
        public Task<string> FavoriteImage(IImageAsset asset) => Execute<ImgurApiResponse<string>, string>(HttpMethod.Post, $"3/image/{asset.Id}/favorite", arg => arg.Data);
        public Task<IImageAsset> GetImage(string assetId) => Execute<ImgurApiResponse<ImgurGaleryAsset>, IImageAsset>(HttpMethod.Get, $"3/image/{assetId}", arg => arg.Data);

        public async Task UploadImage(Stream image)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "3/image")
                {
                    Headers = {Authorization = AuthenticationHeaderValue.Parse($"{IdentityProvider.GetAuthenticationHeader()}")},
                    Content = new StreamContent(image)
                };

                var response = await Client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.Unauthorized) {
                    await IdentityProvider.ReAuthorize();
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
