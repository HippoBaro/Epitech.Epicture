using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.ViewModels.Core;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Epitech.Epicture.ViewModels
{
    internal class GalleryViewModel<TService> : ViewModelBase where TService : IImageClientService, new()
    {
        private ObservableCollection<IImageAsset> _assets = new ObservableCollection<IImageAsset>();
        private string _searchQuery;
        private int _currentPage;

        public IImageClientService ImageClientService { get; set; } = (IImageClientService)Splat.Locator.Current.GetService(typeof(TService));

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

        public ICommand UploadFileCommand => new Command(async () =>
        {
            var selection = await Page.DisplayActionSheet("Upload image", "Cancel", null, "Pick from Camera Roll", "Take photo");
            if (string.IsNullOrEmpty(selection) || selection == "Cancel")
                return;

            await CrossMedia.Current.Initialize();

            MediaFile file = null;
            if (selection == "Take photo")
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Page.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = false,
                    Directory = "ImageTemp",
                    Name = $"{DateTime.UtcNow}.jpg",
                    AllowCropping = false,
                    PhotoSize = PhotoSize.Large,
                    CompressionQuality = 92,
                });
            }
            else
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    CompressionQuality = 92,
                    PhotoSize = PhotoSize.Large,
                });
            }

            if (file == null)
                return;

            try
            {
                await UploadFile(file);
            }
            catch (Exception e)
            {
                await Page.DisplayAlert("error", e.Message, "Ok");
            }
        });

        public override async void OnDisplay()
        {
            base.OnDisplay();
            await Fetch();
        }

        private async Task UploadFile(MediaFile file)
        {
            if (!await EnsureUserIsAuthenticated(ImageClientService.IdentityProvider))
                return;
            try
            {
                await ImageClientService.UploadImage(file.GetStream());
                await Page.DisplayAlert("Success", "It's Aliiivvvvee", "Ok");
            }
            catch (Exception e)
            {
                await Page.DisplayAlert("Error", e.Message, "Ok");
            }
        }

        private async Task Fetch()
        {
            if (IsFetching)
                return;
            IsFetching = true;
            if (CurrentPage == 0)
                Assets.Clear();
            
            var assets = await (string.IsNullOrEmpty(SearchQuery) ? ImageClientService.GetMainGalery(CurrentPage) : ImageClientService.SearchMainGalery(SearchQuery, CurrentPage));
            
            foreach (var asset in assets)
            {
                if (asset.ShouldDisplay)
                    Assets.Add(asset);
            }
            IsFetching = false;
        }
    }
}
