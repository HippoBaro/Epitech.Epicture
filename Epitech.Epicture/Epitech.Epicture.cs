using System;
using Epitech.Epicture.Services;
using Xamarin.Forms;

namespace Epitech.Epicture
{
	public class App : Application
	{
		public App()
		{
            
            // The root page of your application
            var content = new ContentPage
			{
				Title = "Epitech.Epicture",
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						}
					}
				}
			};

			MainPage = new NavigationPage(content);
		}

		protected override async void OnStart()
		{
            var temp = new ImgurClientService();

            var images = await temp.GetMainGalery();
            // Handle when your app starts
        }

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
