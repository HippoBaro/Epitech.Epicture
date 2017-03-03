using System.ComponentModel;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Epitech.Epicture.UWP.Controls;
using Epitech.Epicture.Views.Controls;
using Xamarin.Forms.Platform.UWP;
using Image = Xamarin.Forms.Image;
using ImageSource = Xamarin.Forms.ImageSource;

[assembly: ExportRenderer(typeof(RichImage), typeof(RichImageRenderer))]
namespace Epitech.Epicture.UWP.Controls
{
    public class RichImageRenderer : ImageRenderer
    {
        private MediaPlayer _player;
        private MediaPlayerElement _playerElement;

        public new RichImage Element => base.Element as RichImage;

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            _player = new MediaPlayer()
            {
                AutoPlay = true,
                IsLoopingEnabled = true,
                IsMuted = true
            };
            _playerElement = new MediaPlayerElement()
            {
                AutoPlay = true,
                Visibility = Visibility.Visible,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Stretch = Stretch.UniformToFill,
                Background = new SolidColorBrush(Colors.Brown)
            };
            
            _playerElement.SetMediaPlayer(_player);
            this.Children.Add(_playerElement);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _playerElement.Height = finalSize.Height;
            _playerElement.Width = finalSize.Width;
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(RichImage.RichSource))
            {
                if (Element.RichSource != null && Element.RichSource.ToString().Substring(Element.RichSource.ToString().Length - 3, 3) == "mp4")
                {
                    _player.Source = MediaSource.CreateFromUri(Element.RichSource);
                    _playerElement.Visibility = Visibility.Visible;
                    Control.Visibility = Visibility.Collapsed;
                    _player.Play();
                }
                else if (Element.RichSource != null)
                {
                    //_player.Dispose();
                    _playerElement.Visibility = Visibility.Collapsed;
                    Control.Visibility = Visibility.Visible;
                    Element.Source = ImageSource.FromUri(Element.RichSource);
                }
                else
                {
                    Element.Source = null;
                }
            }
        }
    }
}