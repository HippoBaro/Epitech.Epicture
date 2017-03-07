using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.ViewModels.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.ViewModels
{
    public class ImageDetailViewModel<TService> : ViewModelBase where TService : IImageClientService, new()
    {
        private IImageAsset _imageAsset;
        private bool _isStared;
        private string _assetId;
        private string _comment;

        public IImageAsset ImageAsset
        {
            get { return _imageAsset; }
            set
            {
                _imageAsset = value;
                if (_imageAsset == null)
                    return;
                IsStared = _imageAsset.Favorite;
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
                ImageClientService.GetImage(_assetId).ContinueWith(task => Device.BeginInvokeOnMainThread(() => ImageAsset = task.Result));
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
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
            IsStared = await ImageClientService.FavoriteImage(ImageAsset) != "unfavorited";
        });

        public ICommand NewComment => new Command(async () =>
        {
            if (!await EnsureUserIsAuthenticated(ImageClientService.IdentityProvider))
                return;
            
            try
            {
                await ImageClientService.CommentOnAsset(ImageAsset, Comment);
                AssetId = ImageAsset.Id;
                Comment = null;
            }
            catch (Exception e)
            {
                await Page.DisplayAlert("Error", "Unable to comment", "Dismiss");
            }
        });

        private async Task GetComments()
        {
            try
            {
                var comments = await ImageClientService.GetGalleryAssetComments(ImageAsset);
                Comments.Clear();
                if (comments == null || !comments.Any()) return;
                foreach (var comment in comments)
                    Comments.Add(comment);
            }
            catch (Exception e)
            {
            }

        }
    }
}
