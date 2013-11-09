using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Facebook;
using BitBankWP_places_app.ViewModel;

namespace facebook_windows_phone_sample.Pages
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        private const string AppId = "191969347658165";

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "user_about_me,read_stream"; //,publish_stream

        private readonly FacebookClient _fb = new FacebookClient();

        public FacebookLoginPage()
        {
            InitializeComponent();
        }

        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUrl = GetFacebookLoginUrl(AppId, ExtendedPermissions);
            webBrowser1.Navigate(loginUrl);
        }

        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }

            return _fb.GetLoginUrl(parameters);
        }

        private void webBrowser1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            /*
            FacebookOAuthResult oauthResult;
            if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                var accessToken = oauthResult.AccessToken;
                LoginSucceded(accessToken);
            }
            else
            {
                // user cancelled
                MessageBox.Show(oauthResult.ErrorDescription);
            }
            */
        }

        private void webBrowser1_Navigating(object sender, NavigatingEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                var accessToken = oauthResult.AccessToken;
                LoginSucceded(accessToken);
            }
            else
            {
                // user cancelled
                MessageBox.Show(oauthResult.ErrorDescription);
            }
        }

        private void LoginSucceded(string accessToken)
        {
            var fb = new FacebookClient(accessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var id = (string)result["id"];

                var url = string.Format("/Pages/FacebookInfoPage.xaml?access_token={0}&id={1}", accessToken, id);

                Dispatcher.BeginInvoke(() =>
                {
                    ViewModelLocator.MainStatic.User = new UserViewModel();
                    ViewModelLocator.MainStatic.User.FacebookId = id;
                    ViewModelLocator.MainStatic.User.FacebookToken = accessToken;
                    ViewModelLocator.MainStatic.User.IsLogged = true;
                    GraphApiSample();
                });
            };

            fb.GetAsync("me?fields=id");
        }

        private void GraphApiSample()
        {
            var fb = new FacebookClient(ViewModelLocator.MainStatic.User.FacebookToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();

                Dispatcher.BeginInvoke(() =>
                {
                    ViewModelLocator.MainStatic.User.Username = (string)result["name"];
                    ViewModelLocator.MainStatic.User.FirstName = (string)result["first_name"];
                    ViewModelLocator.MainStatic.User.LastName = (string)result["last_name"];

                    // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
                    // for more info visit http://developers.facebook.com/docs/reference/api
                    string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", 
                        ViewModelLocator.MainStatic.User.FacebookId, 
                        "large", 
                        ViewModelLocator.MainStatic.User.FacebookToken);
                    ViewModelLocator.MainStatic.User.UserImage = profilePictureUrl;

                    var url = string.Format("/Pages/FacebookInfoPage.xaml?access_token={0}&id={1}", ViewModelLocator.MainStatic.User.FacebookToken, ViewModelLocator.MainStatic.User.FacebookId);
                    //Dispatcher.BeginInvoke(() =>
                    //NavigationService.Navigate(new Uri(url, UriKind.Relative));
                    //);
                    NavigationService.GoBack();
                });
            };

            fb.GetAsync("me");
        }

    }
}