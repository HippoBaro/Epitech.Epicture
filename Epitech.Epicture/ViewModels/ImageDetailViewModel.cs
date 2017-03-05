using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Services;
using Epitech.Epicture.ViewModels.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.ViewModels
{
    internal class ImageDetailViewModel : ViewModelBase
    {
        private IImageAsset _imgurGaleryAsset;
        private bool _isStared;
        private string _assetId;

        public IImageAsset ImgurGaleryAsset
        {
            get { return _imgurGaleryAsset; }
            set
            {
                _imgurGaleryAsset = value;
                if (_imgurGaleryAsset == null)
                    return;
                IsStared = _imgurGaleryAsset.Favorite;
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
                ImgurClientService.GetImage(_assetId).ContinueWith(task => Device.BeginInvokeOnMainThread(() => ImgurGaleryAsset = task.Result));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IAssetComment> Comments { get; set; } = new ObservableCollection<IAssetComment>();

        public ImgurClientService ImgurClientService { get; set; }

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
            if (string.IsNullOrEmpty(App.IdentityProvider.IdentityToken))
            {
                Device.OpenUri(App.IdentityProvider.GetAuthorisationUrl());
                var pin = await DisplayInputBox("Authentication", "Fill-in the provided PIN", "Your PIN");
                if (string.IsNullOrWhiteSpace(pin))
                    return;
                try
                {
                    await App.IdentityProvider.Authorize(pin);
                }
                catch (Exception e)
                {
                    await Page.DisplayAlert("Error", e.Message, "Ok");
                    return;
                }
            }
            IsStared = !IsStared;
            IsStared = await ImgurClientService.FavoriteImage(ImgurGaleryAsset) != "unfavorited";
        });

        public ImageDetailViewModel()
        {
            ImgurClientService = new ImgurClientService();
        }

        private async Task GetComments()
        {
            var comments = await ImgurClientService.GetGalleryAssetComments(ImgurGaleryAsset);
            Comments.Clear();
            foreach (var comment in comments)
                Comments.Add(comment);
        }
    }
}
