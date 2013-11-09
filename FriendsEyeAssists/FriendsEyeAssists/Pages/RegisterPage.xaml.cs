using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FriendsEyeAssists.ViewModel;
using System.Windows.Input;

namespace FriendsEyeAssists.Pages
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void RegisterAppbarButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
                //ViewModelLocator.MainStatic.User.UserName = this.Use

                bool result = await ViewModelLocator.MainStatic.BuddyItem.RegisterUser();
                if (result)
                {
                    this.NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Не удалось зарегистрироваться");
                };
            }
            catch { };
        }

        private void CancelAppBarButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.NavigationService.GoBack();
            }
            catch { };
        }

        private void Login_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Password.Focus();
            };
        }

        private void Password_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.AgeText.Focus();
            };
        }
    }
}