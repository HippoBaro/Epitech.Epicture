using System.Collections.Specialized;
using System.Linq;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.ViewModels;
using Epitech.Epicture.Views.Core;
using Xamarin.Forms;
using Epitech.Epicture.Services.Contracts;

namespace Epitech.Epicture.Views
{
    internal class GaleryView<TService> : ContentPageBase<GalleryViewModel<TService>> where TService : IImageClientService, new()
    {
        private readonly StackLayout _stack;
        private readonly ScrollView _scrollView;

        public GaleryView()
        {
            var search = new SearchBar
            {
                Placeholder = "Search",
                HorizontalOptions = LayoutOptions.Fill
            };

            search.SearchButtonPressed += (sender, args) =>
            {
                ViewModel.SearchQuery = search.Text;
                ViewModel.FetchCommand.Execute(true);
            };

            _stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Children = { search }
            };

            _scrollView = new ScrollView
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
                            var item = GetImage((IImageAsset) argsNewItem);
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

            ToolbarItems.Add(new ToolbarItem("Upload", Icon, () => ViewModel.UploadFileCommand.Execute(null)));
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs args)
        {
            foreach (var view1 in _stack.Children.Where(view => view.BindingContext is IImageAsset))
            {
                var stackChild = (RelativeLayout) view1;
                var image = stackChild.Children.First(view => view is Image) as Image;
                if (image == null) continue;
                image.Source = stackChild.Bounds.IntersectsWith(new Rectangle(args.ScrollX, args.ScrollY, _scrollView.Bounds.Width, _scrollView.Bounds.Height).Inflate(0, Bounds.Height * 0.50)) ? ((IImageAsset)stackChild.BindingContext).ContentImageMedium : null;
            }

            if (args.ScrollY > (_scrollView.ContentSize.Height - _scrollView.Height) * 0.70)
                ViewModel.FetchCommand.Execute(true);
        }

        private RelativeLayout GetImage(IImageAsset asset)
        {
            var image = new Image
            {
                Aspect = Aspect.AspectFit,
                BackgroundColor = Color.Accent
            };

            var titleFrame = new StackLayout
            {
                Children = {
                    new Label
                    {
                        Text = asset.Title,
                        TextColor = Color.White
                    }
                },
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(0),
                Padding = new Thickness(10),
                BackgroundColor = Color.Black.MultiplyAlpha(0.7),
                VerticalOptions = LayoutOptions.Start
            };

            var heigh = asset.Ratio > 0 ? Bounds.Width / asset.Ratio : Bounds.Width * asset.Ratio;
            var frame = new RelativeLayout
            {
                Margin = new Thickness(0),
                BindingContext = asset,
                HeightRequest = heigh,
                WidthRequest = Bounds.Width,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    { image, () => new Rectangle(0, 0, Bounds.Width, asset.Ratio > 0 ? Bounds.Width / asset.Ratio : Bounds.Width * asset.Ratio) },
                    { titleFrame, () => new Rectangle(0, 0, image.Width, image.Height / 0.33) }
                }
            };

            image.SizeChanged += (sender, args) =>
            {
                var newheigh = asset.Ratio > 0 ? Bounds.Width / asset.Ratio : Bounds.Width * asset.Ratio;
                frame.HeightRequest = newheigh;
                frame.WidthRequest = Bounds.Width;
            };

            frame.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => await Navigation.PushAsync(new ImageDetailView<TService>(((IImageAsset)frame.BindingContext).Id)))
            });
            return frame;
        }
    }
}
