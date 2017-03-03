using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Epitech.Epicture.Model;
using Epitech.Epicture.Services;
using Epitech.Epicture.ViewModels.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.ViewModels
{
    internal class GalleryViewModel : ViewModelBase
    {
        private ObservableCollection<ImgurGaleryAsset> _assets = new ObservableCollection<ImgurGaleryAsset>();
        private string _searchQuery;
        private int _currentPage;

        public ImgurClientService ImgurClientService { get; set; }

        public ObservableCollection<ImgurGaleryAsset> Assets
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
                if (!string.IsNullOrEmpty(asset.Link))
                Assets.Add(asset);
            }
            IsFetching = false;
        }
    }
}
