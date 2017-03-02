using Epitech.Epicture.ViewModels;
using Epitech.Epicture.ViewModels.Core;
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
