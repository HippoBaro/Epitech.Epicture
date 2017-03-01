using System.Collections.Generic;
using Epitech.Epicture.Model;
using Epitech.Epicture.ViewModels;
using Epitech.Epicture.Views.CellsView;
using Epitech.Epicture.Views.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.Views
{
	internal class GaleryView : ContentPageBase<GalleryViewModel>
	{
	    public List<ImgurGaleryAsset> Assets { get; set; }

		public GaleryView()
		{
		    var search = new SearchBar()
		    {
                Placeholder = "Chercher",
                HorizontalOptions = LayoutOptions.Fill
		    };
		    search.SearchButtonPressed += (sender, args) =>
		    {
		        ViewModel.SearchQuery = search.Text;
		        ViewModel.FetchCommand.Execute(0);
		    };

			var list = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(() => new ImageCellView()),
                HasUnevenRows = true,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Header = search
            };

		    //list.ItemAppearing += (sender, args) =>
		    //{
		    //    if (ViewModel?.Assets == null || ViewModel.Assets.Count < 20) return;
      //          if (ViewModel.Assets.IndexOf((ImgurGaleryAsset)args.Item) >= ViewModel.Assets.Count - 5)
      //              ViewModel.FetchCommand.Execute(++ViewModel.CurrentPage);
		    //};

            list.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(ViewModel.Assets), BindingMode.OneWay));

		    Content = list;
		}
	}
}
