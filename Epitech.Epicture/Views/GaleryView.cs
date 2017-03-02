using System.Collections.Specialized;
using System.Linq;
using Epitech.Epicture.Model;
using Epitech.Epicture.ViewModels;
using Epitech.Epicture.Views.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.Views
{
    internal class GaleryView : ContentPageBase<GalleryViewModel>
    {
        private readonly StackLayout _stack;
        private readonly ScrollView _scrollView;

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
                ViewModel.CurrentPage = 0;
                ViewModel.FetchCommand.Execute(null);
            };
            _stack = new StackLayout()
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Children = { search }
            };

            _scrollView = new ScrollView()
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Content = _stack
            };
            
            ViewModel.Assets.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var argsNewItem in args.NewItems)
                        {
                            var item = GetImage((ImgurGaleryAsset) argsNewItem);
                            if (item != null)
                                _stack.Children.Add(item);
                        }
                        break;
                    default:
                        _stack.Children.Clear();
                        _stack.Children.Add(search);
                        foreach (var argsNewItem in ViewModel.Assets)
                        {
                            var item = GetImage(argsNewItem);
                            if (item != null)
                                _stack.Children.Add(item);
                        }
                        break;
                }
            };

            _scrollView.LayoutChanged += (sender, args) =>
            {
                if (!ViewModel.IsFetching)
                    ScrollView_Scrolled(this, new ScrolledEventArgs(_scrollView.ScrollX, _scrollView.ScrollY));
            };

            _scrollView.Scrolled += ScrollView_Scrolled;
            Content = _scrollView;
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs args)
        {
            foreach (Image stackChild in _stack.Children.Where(view => view is Image))
            {
                void LoadAsset() => stackChild.Source = ((ImgurGaleryAsset) stackChild.BindingContext).ContentImageMedium;
                if (stackChild.Bounds.IntersectsWith(new Rectangle(args.ScrollX, args.ScrollY, _scrollView.Bounds.Width, _scrollView.Bounds.Height).Inflate(0, Bounds.Height * 0.50)))
                    LoadAsset();
                else
                {
                    stackChild.HeightRequest = stackChild.Bounds.Height;
                    stackChild.Source = null;
                }
            }

            if (args.ScrollY > _scrollView.ContentSize.Height * 0.70)
                ViewModel.FetchCommand.Execute(++ViewModel.CurrentPage);
        }

        private Image GetImage(ImgurGaleryAsset asset)
        {
            if (string.IsNullOrEmpty(asset.Link) || asset.IsAlbum)
                return null;
            var image = new Image()
            {
                Aspect = Aspect.AspectFit,
                BackgroundColor = Color.Fuchsia,
                BindingContext = asset,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = Bounds.Width * asset.Ratio,
                WidthRequest = Bounds.Width
            };

            this.MeasureInvalidated += (sender, args) =>
            {
                if (asset.Ratio > 0)
                    image.HeightRequest = Bounds.Width / asset.Ratio;
                else
                    image.HeightRequest = Bounds.Width * asset.Ratio;
                image.WidthRequest = Bounds.Width;
            };

            image.GestureRecognizers.Add(new TapGestureRecognizer(async view =>
            {
                await Navigation.PushAsync(new ImageDetailView((ImgurGaleryAsset)view.BindingContext));
            }));
            return image;
        }
    }
}
