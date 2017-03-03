using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Epitech.Epicture.Model;
using Epitech.Epicture.Model.Core;
using Epitech.Epicture.Services.Core;

namespace Epitech.Epicture.Services
{
    internal class ImgurClientService : ImgurBaseClient
    {
        public Task<List<ImgurGaleryAsset>> GetMainGalery(int page) => GetRessource<ImgurApiResponse<List<ImgurGaleryAsset>>, List<ImgurGaleryAsset>>($"3/gallery/hot/viral/{page}", arg => arg.Data);
        public Task<List<ImgurGaleryAsset>> SearchMainGalery(string query, int page) => GetRessource<ImgurApiResponse<List<ImgurGaleryAsset>>, List<ImgurGaleryAsset>>($"3/gallery/search/viral/{page}?q={Uri.EscapeDataString(query)}", arg => arg.Data);
        public Task<List<ImgurComment>> GetGalleryAssetComments(ImgurGaleryAsset asset) => GetRessource<ImgurApiResponse<List<ImgurComment>>, List<ImgurComment>>($"3/gallery/image/{asset.Id}/comments", arg => arg.Data);

    }
}
