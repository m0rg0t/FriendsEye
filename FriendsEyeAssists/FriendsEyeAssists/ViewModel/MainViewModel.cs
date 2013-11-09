using Buddy;
using FriendsEyeAssists.Model;
using GalaSoft.MvvmLight;
using System;
using System.Windows;
using System.Windows.Threading;

namespace FriendsEyeAssists.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        /// <summary>
        /// 
        /// </summary>
        public BuddyClient client = new BuddyClient(App.API_LOGIN, App.API_PASSWORD);

        public UserItem User = new UserItem();

        public void RegisterUser()
        {
            //Constants
            
            
            //Create a user account
            client.CreateUserAsync((user, state) =>
            {
                //Check that creation succeeded
                if (state.Exception != null) MessageBox.Show("CreateUserAsync Error: " + state.Exception.Message + " " + state.Exception.StackTrace);
                else
                {
                    //Use dispatchers to prevent threading errors when accessing UI objects/methods
                    //Dispatcher.BeginInvoke((Action)(() =>
                    //{
                    //    MessageBox.Show("User creation was a success!");
                    //}));
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("User creation was a success!");
                    });
                }
            }, name: this.User.UserName, password: this.User.UserPassword, 
               //All of the arguments below are optional
                   gender: UserGender.Female, age: 37, email: "test@buddy.com", status: UserStatus.Engaged, fuzzLocation: false,
                   celebrityMode: false, appTag: "WinSDKDocApp", state: string.Empty);
        }
    }
}