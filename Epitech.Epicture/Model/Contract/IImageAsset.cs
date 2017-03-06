using Xamarin.Forms;

namespace Epitech.Epicture.Model.Contract
{
    public interface IImageAsset : IAsset
    {
        bool ShouldDisplay { get; }
        bool Favorite { get; }
        string Title { get; }

        double Ratio { get; }
        ImageSource ContentImageMedium { get; }
        ImageSource ContentImageFull { get; }
    }
}
