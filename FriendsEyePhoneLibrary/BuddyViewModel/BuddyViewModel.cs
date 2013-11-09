using Buddy;
using FriendsEyeAssists.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendsEyePhoneLibrary.BuddyViewModel
{
    public class BuddyViewModel : ViewModelBase
    {
        public BuddyViewModel()
        {
        }

        const string API_LOGIN = "FriendsEye"; //Get it from Buddy's site
        const string API_PASSWORD = "C5A92B64-0583-4ED7-93D5-9C133DFB2904"; //same as above

        public BuddyClient client = new BuddyClient(API_LOGIN, API_PASSWORD);

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
            catch
            {
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
                if (userAuth != null)
                {
                    return true;
                }
                else { return false; };
            }
            catch
            {
                this.Loading = false;
                return false;
            };
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
    }
}
