
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Contract;

namespace Epitech.Epicture.Services.Contracts
{
    public interface IImageClientService : IBaseClient
    {
        Task<List<IImageAsset>> GetMainGalery(int page);
        Task<List<IImageAsset>> SearchMainGalery(string query, int page);
        Task<List<IAssetComment>> GetGalleryAssetComments(IImageAsset asset);
        Task<string> FavoriteImage(IImageAsset asset);
        Task<IImageAsset> GetImage(string assetId);
        Task<IImageAsset> UploadImage(Stream image);
        Task CommentOnAsset(IImageAsset asset, string comment);
    }
}
