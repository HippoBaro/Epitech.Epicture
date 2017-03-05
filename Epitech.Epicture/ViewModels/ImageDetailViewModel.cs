using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.ViewModels.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.ViewModels
{
    internal class ImageDetailViewModel<TService> : ViewModelBase where TService : IImageClientService, new()
    {
        private IImageAsset _imgurAsset;
        private bool _isStared;
        private string _assetId;

        public IImageAsset ImgurAsset
        {
            get { return _imgurAsset; }
            set
            {
                _imgurAsset = value;
                if (_imgurAsset == null)
                    return;
                IsStared = _imgurAsset.Favorite;
                FetchComments.Execute(null);
            }
        }

        public string AssetId
        {
            get { return _assetId; }
            set
            {
                _assetId = value;
                if (string.IsNullOrEmpty(_assetId))
                    return;
                ImageClientService.GetImage(_assetId).ContinueWith(task => Device.BeginInvokeOnMainThread(() => ImgurAsset = task.Result));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IAssetComment> Comments { get; set; } = new ObservableCollection<IAssetComment>();

        public IImageClientService ImageClientService { get; set; } = (IImageClientService)Splat.Locator.Current.GetService(typeof(TService));

        public ICommand FetchComments => new Command(async () => await GetComments());

        public bool IsStared
        {
            get { return _isStared; }
            set
            {
                _isStared = value;
                OnPropertyChanged();
            }
        }

        public ICommand StarCommand => new Command(async o =>
        {
            if (!await EnsureUserIsAuthenticated(ImageClientService.IdentityProvider))
                return;
            IsStared = !IsStared;
            IsStared = await ImageClientService.FavoriteImage(ImgurAsset) != "unfavorited";
        });

        private async Task GetComments()
        {
            var comments = await ImageClientService.GetGalleryAssetComments(ImgurAsset);
            Comments.Clear();
            foreach (var comment in comments)
                Comments.Add(comment);
        }
    }
}
