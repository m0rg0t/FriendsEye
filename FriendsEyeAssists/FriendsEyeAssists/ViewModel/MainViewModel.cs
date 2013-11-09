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
            User = new UserItem();
        }

        private bool _loading = false;
        /// <summary>
        /// When data is loading
        /// </summary>
        public bool Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                RaisePropertyChanged("Loading");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public BuddyClient client = new BuddyClient(App.API_LOGIN, App.API_PASSWORD);


        private UserItem _user = new UserItem();
        /// <summary>
        /// 
        /// </summary>
        public UserItem User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
            }
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoginUser()
        {
            this.Loading = true;
            try
            {
                AuthenticatedUser userAuth = await client.LoginAsync(this.User.UserName, this.User.Password);
                this.User.BuddyUser = userAuth;
                this.Loading = false;
                return true;
            }
            catch {
                this.Loading = false;
                return false;
            };
        }


        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        public async Task<bool> RegisterUser()
        {
            this.Loading = true;
            try
            {
                AuthenticatedUser userAuth = await client.CreateUserAsync(name: this.User.UserName, password: this.User.Password,
                       age: this.User.Age, email: this.User.Email, fuzzLocation: false, appTag: "FriendsEye");
                this.User.BuddyUser = userAuth;
                this.Loading = false;
                if (userAuth!=null) {
                return true; }
                else { return false; };
            }
            catch {
                this.Loading = false;
                return false;
            };
            return true;
        /*
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
            }, name: this.User.UserName, password: this.User.Password, 
               //All of the arguments below are optional
                   gender: UserGender.Female, age: this.User.Age, email: this.User.Email, fuzzLocation: false, appTag: "FriendsEye", state: string.Empty);*/
        }


    }
}