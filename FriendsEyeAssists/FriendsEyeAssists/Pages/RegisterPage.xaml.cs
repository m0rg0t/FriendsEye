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

namespace FriendsEyeAssists.Pages
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterAppbarButton_Click(object sender, EventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.RegisterUser();
                this.NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
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
    }
}