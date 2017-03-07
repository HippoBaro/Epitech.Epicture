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

            var textComment = new Editor
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 250
            };

            var list = new ListView
            {
                Header = new StackLayout
                {
                    Children =
                    {
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
                },
                Footer = new StackLayout
                {
                    Children =
                    {
                        textComment,
                        new Button
                        {
                            Text = "New comment",
                            HorizontalOptions = LayoutOptions.Center,
                            Command = ViewModel.NewComment
                        }
                    },
                    HorizontalOptions = LayoutOptions.Fill
                }
            };

            list.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(ViewModel.Comments)));
            star.SetBinding(Image.SourceProperty, new Binding(nameof(ViewModel.IsStared), BindingMode.OneWay, new StartedToAssetValueConverter()));
            textComment.SetBinding(Editor.TextProperty, new Binding(nameof(ViewModel.Comment), BindingMode.TwoWay));

            ViewModel.PropertyChanged += (sender, args) =>
            {
                img.Source = ViewModel.ImageAsset.ContentImageFull;
            };

            Content = list;
        }
    }
}
