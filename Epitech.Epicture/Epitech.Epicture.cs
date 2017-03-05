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

		    MainPage = new TabbedPage
		    {
		        Children =
		        {
		            new NavigationPage(new GaleryView<ImgurClientService>()) { Title = "Imgur" }
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
