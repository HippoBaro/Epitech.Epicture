using System;
using System.Globalization;
using Xamarin.Forms;

namespace Epitech.Epicture.ValueConverters
{
    internal class StartedToAssetValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ImageSource.FromResource((bool)value ? "Epitech.Epicture.Ressources.star_filled.png" : "Epitech.Epicture.Ressources.star_empty.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
