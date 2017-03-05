using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.ValueConverters;
using Epitech.Epicture.ViewModels;
using Epitech.Epicture.Views.Core;
using Xamarin.Forms;

namespace Epitech.Epicture.Views
{
    internal class ImageDetailView<TService> : ContentPageBase<ImageDetailViewModel<TService>> where TService : IImageClientService, new()
    {
        public ImageDetailView(string assetId)
        {
            ViewModel.AssetId = assetId;
            var star = new Image
            {
                Aspect = Aspect.AspectFit,
                BackgroundColor = Color.Lime,
                GestureRecognizers = {new TapGestureRecognizer { Command = ViewModel.StarCommand }},
                VerticalOptions = LayoutOptions.Center
            };

            var img = new Image {HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Fill, Aspect = Aspect.AspectFit};

            var list = new ListView
            {
                Header = new StackLayout
                {
                    Children = {
                        img,
                        new StackLayout
                        {
                            HorizontalOptions = LayoutOptions.Fill,
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                star,
                                new Label
                                {
                                    Text = "Comments",
                                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                                    Margin = new Thickness(15, 10),
                                    VerticalOptions = LayoutOptions.Center
                                }
                            }
                        },
                    }
                },
                ItemTemplate = new DataTemplate(typeof(TextCell))
                {
                    Bindings =
                    {
                        {TextCell.TextProperty, new Binding(nameof(IAssetComment.Author))},
                        {TextCell.DetailProperty, new Binding(nameof(IAssetComment.Comment))}
                    }
                }
            };

            list.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(ViewModel.Comments)));
            star.SetBinding(Image.SourceProperty, new Binding(nameof(ViewModel.IsStared), BindingMode.OneWay, new StartedToAssetValueConverter()));

            ViewModel.PropertyChanged += (sender, args) =>
            {
                img.Source = ViewModel.ImgurAsset.ContentImageFull;
            };

            Content = list;
        }
    }
}
