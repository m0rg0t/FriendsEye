using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BitBankWP_places_app.ViewModel;
using BitBankWP_places_app.Model;
using Parse;
using System.Windows.Controls.Primitives;
using Facebook;
using Coding4Fun.Toolkit.Controls;
using Windows.Phone.Speech.Synthesis;

namespace BitBankWP_places_app
{
    public partial class PanoramaMainPage : PhoneApplicationPage
    {
        public PanoramaMainPage()
        {
            InitializeComponent();
        }

        Popup browserPopup = new Popup();

        private async void LoginTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                var browser = new WebBrowser() { Name="browser", Width=480, Height=800};
                browserPopup.Child = browser;
                browserPopup.IsOpen = true;
                //this.NavigationService.Navigate(new Uri("/Pages/FacebookLoginPage.xaml", UriKind.Relative));
                ParseUser user = await ParseFacebookUtils.LogInAsync(browser, null);
                var fb = new FacebookClient();
                fb.AccessToken = ParseFacebookUtils.AccessToken;
                var me = await fb.GetTaskAsync("me");

                if (user.IsAuthenticated)
                {
                    IDictionary<string, object> results = (IDictionary<string, object>)me;
                    ViewModelLocator.MainStatic.User.IsLogged = true;
                    ViewModelLocator.MainStatic.User.FacebookId = (string)results["id"];
                    ViewModelLocator.MainStatic.User.FacebookToken = ParseFacebookUtils.AccessToken;

                    ViewModelLocator.MainStatic.User.Username = (string)results["name"];
                    ViewModelLocator.MainStatic.User.FirstName = (string)results["first_name"];
                    ViewModelLocator.MainStatic.User.LastName = (string)results["last_name"];

                    ViewModelLocator.MainStatic.User.ObjectId = user.ObjectId.ToString();

                    // available picture types: square (50x50), small (50xvariable height), large (about 200x variable height) (all size in pixels)
                    // for more info visit http://developers.facebook.com/docs/reference/api
                    string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}",
                        ViewModelLocator.MainStatic.User.FacebookId,
                        "large",
                        ViewModelLocator.MainStatic.User.FacebookToken);
                    ViewModelLocator.MainStatic.User.UserImage = profilePictureUrl;
                };

                browserPopup.IsOpen = false;
            }
            catch {
                browserPopup.IsOpen = false;
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                //NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
                InputPrompt input = new InputPrompt();
                input.Completed += input_Completed;
                input.Title = "Поиск";
                input.Message = "ВВедите текст для поиска:";
                input.Show();
            }
            catch { };
        }

        private void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            /*try
            {
                ViewModelLocator.MainStatic.SearchQuery = e.Result.ToString();
                MainPanorama.DefaultItem = MainPanorama.Items[2];
                ViewModelLocator.MainStatic.LoadSearchPlaces();
            }
            catch { };*/
        }

        private void MapTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                this.NavigationService.Navigate(new Uri("/Pages/MapPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private async void NearestTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPanorama.DefaultItem = MainPanorama.Items[1];
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.PhotoItems.Count() < 1)
                {
                    ViewModelLocator.MainStatic.LoadData();
                };                
            }
            catch { };

            //SpeechSynthesizer synth = new SpeechSynthesizer();
            //await synth.SpeakTextAsync("Насекомые (лат. Insécta) — класс беспозвоночных членистоногих животных. Вместе с многоножками относятся к подтипу трахейнодышащих. Название класса происходит от глагола «сечь» (насекать) и представляет собой кальку с французского «insecte» (латинского insectum), означающего «животное с насечками». Тело насекомых покрыто хитинизированной кутикулой, образующей экзоскелет, и состоит из трёх отделов: головы, груди и брюшка.");
        }

        private void AddTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //Добавить место
            /*try
            {
                if (ViewModelLocator.MainStatic.User.IsLogged) {
                    this.NavigationService.Navigate(new Uri("/Pages/AddPhotoPage.xaml", UriKind.Relative));
                } else {
                    MessageBox.Show("Для добавления отзыва вы должны авторизоваться.");
                    this.NavigationService.Navigate(new Uri("/Pages/FacebookLoginPage.xaml", UriKind.Relative));
                };                
            }
            catch { };*/

            try
            {
                if (ViewModelLocator.MainStatic.User.IsLogged)
                {
                    if (ViewModelLocator.MainStatic.PhotoItems.FirstOrDefault()!=null)
                    {
                    ViewModelLocator.MainStatic.CurrentItem = ViewModelLocator.MainStatic.PhotoItems.FirstOrDefault();
                    //this.NavigationService.Navigate(new Uri("/Pages/AddPhotoPage.xaml", UriKind.Relative));
                    this.NavigationService.Navigate(new Uri("/Pages/AddCommentPage.xaml", UriKind.Relative));
                    } else {
                        MessageBox.Show("Нет изображений для помощи");
                    };
                }
                else
                {
                    MessageBox.Show("Для добавления отзыва вы должны авторизоваться.");
                    this.NavigationService.Navigate(new Uri("/Pages/FacebookLoginPage.xaml", UriKind.Relative));
                };
            }
            catch { };
        }

        private void CategoriesList_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentItem = (this.UnfinishedList.SelectedItem as AssistsPhoto);
                this.NavigationService.Navigate(new Uri("/Pages/ViewPHotoPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void PrivacyPolicyMenu_Click(object sender, EventArgs e)
        {
            try
            {
                var messagePrompt = new MessagePrompt
                {
                    Title = "Политика конфиденциальности",
                    Body = new TextBlock
                    {
                        Text = "Приложение не собирает никаких данных без вашего ведома.\nПриложение не собирает и не хранит информацию, которая связана с определенным именем. Мы также делаем все возможное, чтобы обезопасить хранимые данные.\nПринимая условия, которые включают эту политику вы соглашаетесь с данной политикой конфиденциальности.\nКонтакты m0rg0t.Anton@gmail.com",
                        MaxHeight = 500,
                        TextWrapping = TextWrapping.Wrap
                    },
                    IsAppBarVisible = false,
                    IsCancelVisible = false
                };
                messagePrompt.Show();
            }
            catch { };
        }

        private void RadSlideHubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                this.NavigationService.Navigate(new Uri("/Pages/ProfilePage.xaml", UriKind.Relative));
            }
            catch { };
        }
    }
}