using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epitech.Epicture.Model;
using Epitech.Epicture.Services;

namespace Epitech.Epicture.ViewModels
{
    internal class GalleryViewModel : ViewModelBase
    {
        private ObservableCollection<ImgurGaleryAsset> _assets;

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

        public GalleryViewModel()
        {
            ImgurClientService = new ImgurClientService();
        }

        public override async void OnDisplay()
        {
            base.OnDisplay();
            var assets = await ImgurClientService.GetMainGalery();
            Assets = new ObservableCollection<ImgurGaleryAsset>(assets.Where(asset => !asset.IsAlbum && !asset.IsAd && !string.IsNullOrEmpty(asset.Link)));
        }
    }
}
