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
			var list = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(() => new ImageCellView()),
                HasUnevenRows = true,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            list.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(ViewModel.Assets), BindingMode.OneWay));
		    Content = list;
		}
	}
}
