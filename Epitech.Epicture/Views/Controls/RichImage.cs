using System;
using Xamarin.Forms;

namespace Epitech.Epicture.Views.Controls
{
    public class RichImage : Image
    {
        public static readonly BindableProperty RichSourceProperty = BindableProperty.Create("RichSource", typeof(Uri), typeof(Image));

        public Uri RichSource
        {
            get { return (Uri)GetValue(RichSourceProperty); }
            set { SetValue(RichSourceProperty, value); }
        }

    }
}
