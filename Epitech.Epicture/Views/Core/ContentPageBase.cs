using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epitech.Epicture.ViewModels;
using Xamarin.Forms;

namespace Epitech.Epicture.Views.Core
{
    internal class ContentPageBase<TViewModel> : ContentPage where TViewModel : ViewModelBase, new()
    {
        private TViewModel _viewModel;

        protected TViewModel ViewModel => _viewModel ?? (_viewModel = new TViewModel());

        public ContentPageBase()
        {
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            ViewModel.OnDisplay();
            base.OnAppearing();
        }
    }
}
