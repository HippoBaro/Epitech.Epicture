using Epitech.Epicture.Views;
using Xamarin.Forms;

namespace Epitech.Epicture
{
	public class App : Application
	{
		public App()
		{
		    MainPage = new TabbedPage
		    {
		        Children =
		        {
		            new NavigationPage(new GaleryView()) { Title = "Gallery" }
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
