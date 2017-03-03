using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Epitech.Epicture.Model;
using Epitech.Epicture.Services;
using Epitech.Epicture.ViewModels.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.ViewModels
{
    internal class ImageDetailViewModel : ViewModelBase
    {
        private ImgurGaleryAsset _imgurGaleryAsset;
        private bool _isStared;

        public ImgurGaleryAsset ImgurGaleryAsset
        {
            get { return _imgurGaleryAsset; }
            set
            {
                _imgurGaleryAsset = value;
                if (_imgurGaleryAsset == null)
                    return;
                FetchComments.Execute(null);
            }
        }

        public ObservableCollection<ImgurComment> Comments { get; set; } = new ObservableCollection<ImgurComment>();

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

        public ICommand StarCommand => new Command(o =>
        {
            IsStared = !IsStared;
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
