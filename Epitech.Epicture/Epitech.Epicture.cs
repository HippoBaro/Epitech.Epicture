using Epitech.Epicture.Services.Flickr;
using Epitech.Epicture.Services.Imgur;
using Epitech.Epicture.Views;
using Splat;
using Xamarin.Forms;

namespace Epitech.Epicture
{
	public class App : Application
	{
        public App()
		{
            Locator.CurrentMutable.RegisterLazySingleton(() => new ImgurClientService(), typeof(ImgurClientService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new FlickrClientService(), typeof(FlickrClientService));

            MainPage = new TabbedPage
		    {
		        Children =
		        {
		            //new NavigationPage(new GaleryView<ImgurClientService>()) { Title = "Imgur" },
                    new NavigationPage(new GaleryView<FlickrClientService>()) { Title = "Flickr" }
                }
		    };
		}

		protected override void OnStart()
		{
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
