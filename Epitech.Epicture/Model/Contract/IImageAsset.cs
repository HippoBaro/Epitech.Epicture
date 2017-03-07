using Xamarin.Forms;

namespace Epitech.Epicture.Model.Contract
{
    public interface IImageAsset : IAsset
    {
        /// <summary>
        /// A computed value defining if this asset should be visible to the user.
        /// For example, this propretie returns false for Ad on Imgur.
        /// </summary>
        bool ShouldDisplay { get; }

        /// <summary>
        /// A value that indicate if this asset is a user favorite.
        /// If no user is authenticated, Favorite is always false.
        /// </summary>
        bool Favorite { get; }

        /// <summary>
        /// This asset's title
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The computed ratio for this asset
        /// </summary>
        double Ratio { get; }

        /// <summary>
        /// The cached medium-sized image source of this asset. This image source should be prefered for listviews.
        /// Because multiple assets can saturate memory, the underlying ressource is a WeakReference.
        /// When an asset is not visible to the user, this reference should be set to null to allow the garbage collector to free memory.
        /// </summary>
        ImageSource ContentImageMedium { get; }

        /// <summary>
        /// The cached image full-sized source of this asset.
        /// Because multiple assets can saturate memory, the underlying ressource is a WeakReference.
        /// When an asset is not visible to the user, this reference should be set to null to allow the garbage collector to free memory.
        /// </summary>
        ImageSource ContentImageFull { get; }
    }
}
