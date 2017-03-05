using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Epitech.Epicture.Properties;
using Epitech.Epicture.Services.Contracts;
using Xamarin.Forms;

namespace Epitech.Epicture.ViewModels.Core
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public Page Page { get; set; }
        private bool _isFetching;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public bool IsFetching
        {
            get { return _isFetching; }
            set
            {
                _isFetching = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnDisplay() { }

        public Task<string> DisplayInputBox(string title, string inputDescription, string placeholder)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = title, HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = inputDescription };
            var txtInput = new Entry { Placeholder = placeholder };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = txtInput.Text;
                await Page.Navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                await Page.Navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(null);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage {Content = layout};
            Page.Navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }

        protected virtual async Task<bool> EnsureUserIsAuthenticated(IOAuthIdentityProvider identityProvider)
        {
            try
            {
                if (!string.IsNullOrEmpty(identityProvider.IdentityToken)) return true;
                if (!await Page.DisplayAlert("Authentication", "You need to be authenticated to performs this action", "Authenticate", "Dismiss"))
                    return false;
                string pin = null;
                Device.OpenUri(await identityProvider.GetAuthorisationUrl());
                if (identityProvider.NeedUserInput)
                {
                    pin = await DisplayInputBox("Authentication", "Fill-in the provided PIN", "Your PIN");
                    if (string.IsNullOrWhiteSpace(pin))
                        return false;
                }
                else if (!await Page.DisplayAlert("Authentication", "Authorize Flickr before continuing.", "Complete authentication", "Dismiss"))
                    return false;
                await identityProvider.Authorize(pin);
            }
            catch (Exception e)
            {
                await Page.DisplayAlert("Error", e.Message, "Ok");
                return false;
            }
            return true;
        }
    }
}
