using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epitech.Epicture.Model;
using Xamarin.Forms;

namespace Epitech.Epicture.Views.CellsView
{
    public class ImageCellView : ViewCell
    {
        public ImageCellView()
        {
            var image = new Image()
            {
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.Fuchsia
            };

            image.SetBinding(Image.SourceProperty, new Binding(nameof(ImgurGaleryAsset.Link), BindingMode.OneWay));
            image.SetBinding(VisualElement.HeightProperty, new Binding(nameof(ImgurGaleryAsset.Height), BindingMode.OneWay));
            View = image;
        }
    }
}
