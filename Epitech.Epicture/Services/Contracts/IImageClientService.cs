
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Contract;

namespace Epitech.Epicture.Services.Contracts
{
    public interface IImageClientService : IBaseClient
    {
        /// <summary>
        /// Get a list of publicly available images from this Service provider.
        /// This method doesn't need Authentication.
        /// This method provide paging.
        /// </summary>
        /// <param name="page">The 0-based page your request concerns</param>
        /// <returns>A list of assets</returns>
        Task<List<IImageAsset>> GetMainGalery(int page);

        /// <summary>
        /// Get a list of publicly available images from this Service provider based a search query.
        /// This method doesn't need Authentication.
        /// This method provide paging.
        /// </summary>
        /// <param name="query">Text that should be condidered when querying the service</param>
        /// <param name="page">The 0-based page your request concerns</param>
        /// <returns>A list of assets</returns>
        Task<List<IImageAsset>> SearchMainGalery(string query, int page);

        /// <summary>
        /// Get all root-level comments concerning a sepecific asset.
        /// This method doesn't need Authentication.
        /// </summary>
        /// <param name="asset">The image you want comment for</param>
        /// <returns>A list of comments</returns>
        Task<List<IAssetComment>> GetGalleryAssetComments(IImageAsset asset);

        /// <summary>
        /// Favorite ou unfavorite a specific asset.
        /// This method needs an authenticated user.
        /// If the authenticated user has already favorited this asset, it will be unfavorited.
        /// </summary>
        /// <param name="asset">The asset to favorite or unfavorite</param>
        /// <returns>A string representing the new favorited state of the asset</returns>
        Task<string> FavoriteImage(IImageAsset asset);

        /// <summary>
        /// Retreive all the informations concerning a specific asset. Depending on services provider, this is necessary when accessing an image in order to get all the available image's sources.
        /// This method doesn't need Authentication.
        /// </summary>
        /// <param name="assetId">the id of the asset.</param>
        /// <returns>The full image informations</returns>
        Task<IImageAsset> GetImage(string assetId);

        /// <summary>
        /// Upload an image to this service provider. 
        /// This method needs an authenticated user.
        /// </summary>
        /// <param name="image">The image file's stream to upload</param>
        /// <returns>The full uploaded image informations</returns>
        Task<IImageAsset> UploadImage(Stream image);

        /// <summary>
        /// Post a new user provided comment on an asset.
        /// This method needs an authenticated user.
        /// </summary>
        /// <param name="asset">The asset to post a comment on</param>
        /// <param name="comment">The comment to post</param>
        /// <returns></returns>
        Task CommentOnAsset(IImageAsset asset, string comment);
    }
}
