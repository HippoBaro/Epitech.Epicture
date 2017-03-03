﻿using Epitech.Epicture.Model;
using Epitech.Epicture.ValueConverters;
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

            var star = new Image
            {
                Aspect = Aspect.AspectFit,
                BackgroundColor = Color.Lime,
                GestureRecognizers = {new TapGestureRecognizer { Command = ViewModel.StarCommand }},
                VerticalOptions = LayoutOptions.Center
            };

            var list = new ListView
            {
                Header = new StackLayout
                {
                    Children = {
                        new Image { Source = asset.ContentImageFull, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Fill, Aspect = Aspect.AspectFit },
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
                        {TextCell.TextProperty, new Binding(nameof(ImgurComment.Author))},
                        {TextCell.DetailProperty, new Binding(nameof(ImgurComment.Comment))}
                    }
                }
            };

            list.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(ViewModel.Comments)));
            star.SetBinding(Image.SourceProperty, new Binding(nameof(ViewModel.IsStared), BindingMode.OneWay, new StartedToAssetValueConverter()));

            Content = list;
        }
    }
}
