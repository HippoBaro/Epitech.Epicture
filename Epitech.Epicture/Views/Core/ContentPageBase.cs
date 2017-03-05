using Epitech.Epicture.ViewModels.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.Views.Core
{
    internal class ContentPageBase<TViewModel> : ContentPage where TViewModel : ViewModelBase, new()
    {
        private TViewModel _viewModel;

        protected TViewModel ViewModel => _viewModel ?? (_viewModel = new TViewModel { Page = this });

        public ContentPageBase()
        {
            BindingContext = ViewModel;
            ViewModel.OnDisplay();
        }
    }
}
