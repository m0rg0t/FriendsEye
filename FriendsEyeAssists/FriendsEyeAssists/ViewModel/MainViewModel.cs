using Buddy;
using FriendsEyeAssists.Model;
using GalaSoft.MvvmLight;
using System;
using System.Threading.Tasks;
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

        private bool _loading = false;
        /// <summary>
        /// When data is loading
        /// </summary>
        public bool Loading
        {
            get { return _loading; }
            set { 
                _loading = value;
                RaisePropertyChanged("Loading");
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        public BuddyClient client = new BuddyClient(App.API_LOGIN, App.API_PASSWORD);

        /// <summary>
        /// Main user in app
        /// </summary>
        public UserItem User = new UserItem();

        public async Task<bool> LoginUser()
        {
            AuthenticatedUser userAuth = await client.LoginAsync(this.User.UserName, this.User.UserPassword);
            this.User.BuddyUser = userAuth;
            return true;
        }
        

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        public void RegisterUser()
        {
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
                   gender: UserGender.Female, age: this.User.Age, email: "test@buddy.com", fuzzLocation: false, appTag: "FriendsEye", state: string.Empty);
        }


    }
}