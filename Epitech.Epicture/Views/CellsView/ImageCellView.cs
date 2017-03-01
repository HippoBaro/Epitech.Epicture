using System;
using Epitech.Epicture.Model;
using Xamarin.Forms;

namespace Epitech.Epicture.Views.CellsView
{
    public class ImageCellView : ViewCell
    {
        private Image _image;

        public ImageCellView()
        {
            _image = new Image
            {
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.Fuchsia
            };

            _image.SetBinding(Image.SourceProperty, new Binding(nameof(ImgurGaleryAsset.Link), BindingMode.OneWay));
            View = _image;
        }



        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    var imgurGaleryAsset = BindingContext as ImgurGaleryAsset;
        //    if (imgurGaleryAsset != null)
        //        _image.Source = new UriImageSource()
        //        {
        //            Uri = new Uri(imgurGaleryAsset.Link)
        //        };
        //}
    }
}
