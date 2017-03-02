using Epitech.Epicture.Model;
using Epitech.Epicture.ViewModels;
using Epitech.Epicture.Views.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.Views
{
    internal class ImageDetailView : ContentPageBase<ImageDetailViewModel>
    {
        public ImageDetailView(ImgurGaleryAsset asset)
        {
            ViewModel.ImgurGaleryAsset = asset;

            var list = new ListView
            {
                Header = new Image
                {
                    Source = asset.ContentImageFull,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Fill,
                    Aspect = Aspect.AspectFit
                },
                ItemTemplate = new DataTemplate(typeof(TextCell))
                {
                    Bindings =
                    {
                        {TextCell.TextProperty, new Binding(nameof(ImgurComment.Author))},
                        {TextCell.DetailProperty, new Binding(nameof(ImgurComment.Comment))}
                    }
                }
            };

            list.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(ViewModel.Comments)));

            Content = list;
        }
    }
}
