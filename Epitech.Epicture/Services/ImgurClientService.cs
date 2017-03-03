using System;
using System.Collections.Generic;
using System.Net.Http;
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
    }
}
