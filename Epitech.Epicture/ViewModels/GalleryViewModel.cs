using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Services;
using Epitech.Epicture.ViewModels.Core;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Epitech.Epicture.ViewModels
{
    internal class GalleryViewModel : ViewModelBase
    {
        private ObservableCollection<IImageAsset> _assets = new ObservableCollection<IImageAsset>();
        private string _searchQuery;
        private int _currentPage;

        public ImgurClientService ImgurClientService { get; set; }

        public ObservableCollection<IImageAsset> Assets
        {
            get { return _assets; }
            set
            {
                _assets = value;
                OnPropertyChanged();
            }
        }

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                CurrentPage = 0;
                OnPropertyChanged();
            }
        }

        public ICommand FetchCommand => new Command(async () => await Fetch());

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public ICommand UploadFileCommand => new Command<MediaFile>(async file =>
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
            try
            {
                await ImgurClientService.UploadImage(file.GetStream());
                await Page.DisplayAlert("Success", "It's Aliiivvvvee", "Ok");
            }
            catch (Exception e)
            {
                await Page.DisplayAlert("Error", e.Message, "Ok");
            }
        });

        public GalleryViewModel()
        {
            ImgurClientService = new ImgurClientService();
        }

        public override async void OnDisplay()
        {
            base.OnDisplay();
            await Fetch();
        }

        private async Task Fetch()
        {
            if (IsFetching)
                return;
            IsFetching = true;
            if (CurrentPage == 0)
                Assets.Clear();
            
            var assets = await (string.IsNullOrEmpty(SearchQuery) ? ImgurClientService.GetMainGalery(CurrentPage) : ImgurClientService.SearchMainGalery(SearchQuery, CurrentPage));
            
            foreach (var asset in assets)
            {
                if (asset.ShouldDisplay)
                    Assets.Add(asset);
            }
            IsFetching = false;
        }
    }
}
