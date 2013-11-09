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
using FriendsEyeAssists.ViewModel;

namespace FriendsEyeAssists
{
    public partial class LoginPage : PhoneApplicationPage
    {
        // Constructor
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void StackPanel_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                if ((this.Login.Text!="") && (this.Password.Password!="")) {
                    await ViewModelLocator.MainStatic.LoginUser();
                    try
                    {
                        this.NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
                    }
                    catch { };
                };
            }
            catch { };
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.NavigationService.Navigate(new Uri("/Pages/RegisterPage.xaml", UriKind.Relative));
            }
            catch { };
        }
    }
}