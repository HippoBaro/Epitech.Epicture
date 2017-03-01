using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Epitech.Epicture.Model;
using Epitech.Epicture.Services;
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

        public ICommand FetchCommand => new Command<int>(async page => await Fetch(page));

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
            await Fetch(CurrentPage);
        }

        private async Task Fetch(int page)
        {
            if (page == 0)
                Assets.Clear();
            
            var assets = string.IsNullOrEmpty(SearchQuery) ? ImgurClientService.GetMainGalery(page) : ImgurClientService.SearchMainGalery(SearchQuery, page);

            foreach (var asset in await assets)
            {
                if (!string.IsNullOrEmpty(asset.Link))
                Assets.Add(asset);
            }
        }
    }
}
