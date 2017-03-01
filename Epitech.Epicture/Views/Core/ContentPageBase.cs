using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epitech.Epicture.ViewModels;
using Xamarin.Forms;

namespace Epitech.Epicture.Views.Core
{
    internal class ContentPageBase<TViewModel> : ContentPage where TViewModel : ViewModelBase
    {
        private readonly GalleryViewModel _viewModelBase;

        protected TViewModel ViewModel { get; set; }

        public ContentPageBase()
        {
            BindingContext = _viewModelBase = new GalleryViewModel();
        }

        protected override void OnAppearing()
        {
            _viewModelBase.OnDisplay();
            base.OnAppearing();
        }
    }
}
