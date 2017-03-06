using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Model.Flickr;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.Services.Flickr.Core;
using Newtonsoft.Json.Linq;

namespace Epitech.Epicture.Services.Flickr
{
    public class FlickrClientService : FlickrBaseClient, IImageClientService
    {
        public Task<List<IImageAsset>> GetMainGalery(int page)
            =>
                Execute<JObject, List<IImageAsset>>(HttpMethod.Get, "flickr.photos.getRecent",
                    new Dictionary<string, string> {{"page", (++page).ToString()}, {"extras", "url_l"}}, "photo", arg =>
                    {
                        var tr = arg.Data.Children().Last().First;
                        return new List<IImageAsset>(tr.ToObject<List<FlickrGaleryAsset>>());
                    });

        public Task<List<IImageAsset>> SearchMainGalery(string query, int page)
            => Execute<JObject, List<IImageAsset>>(HttpMethod.Get, "flickr.photos.search",
                new Dictionary<string, string> {{"text", query}, {"page", (++page).ToString()}, {"extras", "url_l"}, {"sort", "interestingness-desc"}},
                "photo", arg =>
                {
                    var tr = arg.Data.Children().Last().First;
                    return new List<IImageAsset>(tr.ToObject<List<FlickrGaleryAsset>>());
                });

        public Task<List<IAssetComment>> GetGalleryAssetComments(IImageAsset asset)
            => Execute<JObject, List<IAssetComment>>(HttpMethod.Get, "flickr.photos.comments.getList",
                new Dictionary<string, string> {{"photo_id", asset.Id}}, "comment", arg =>
                {
                    if (arg.Data.Last.Count() == 1)
                        return null;
                    var ret = arg.Data.Last.First.ToObject<List<FlickrComment>>();
                    return new List<IAssetComment>(ret);
                });

        public Task<string> FavoriteImage(IImageAsset asset)
        {
            if (asset.Favorite)
                return ExecuteNoReturn(HttpMethod.Get, "flickr.favorites.remove", new Dictionary<string, string> {{"photo_id", asset.Id}}, "comment",
                    arg => "unfavorited");
            return ExecuteNoReturn(HttpMethod.Get, "flickr.favorites.add", new Dictionary<string, string> {{"photo_id", asset.Id}}, "comment",
                arg => "favorited");
        }

        public Task<IImageAsset> GetImage(string assetId)
            => Execute<JObject, IImageAsset>(HttpMethod.Get, "flickr.photos.getInfo", new Dictionary<string, string> {{"photo_id", assetId}}, "",
                async (arg) =>
                {
                    var ret = arg.Data.ToObject<FlickrGaleryAsset>();
                    ret.Sources = await GetImageSizes(ret);
                    return ret;
                });

        public Task UploadImage(Stream image)
        {
            throw new Exception("Flick API is monumentally bad. So not uploading.");
        }

        private Task<List<FlickrAssetSource>> GetImageSizes(FlickrGaleryAsset asset)
            => Execute<JObject, List<FlickrAssetSource>>(HttpMethod.Get, "flickr.photos.getSizes",
                new Dictionary<string, string> {{"photo_id", asset.Id}}, "size", arg =>
                {
                    var ret = arg.Data.Last.First.ToObject<List<FlickrAssetSource>>();
                    return ret;
                });
    }
}
