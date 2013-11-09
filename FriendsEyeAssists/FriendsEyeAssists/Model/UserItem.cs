using Buddy;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriendsEyeAssists.Model
{
    public class UserItem: ViewModelBase
    {
        public UserItem()
        {
        }

        private string _userPassword = "";
        /// <summary>
        /// User password
        /// </summary>
        public string UserPassword
        {
            get { return _userPassword; }
            set { 
                _userPassword = value;
                RaisePropertyChanged("UserPassword");
            }
        }        

        private string _userName = "";
        /// <summary>
        /// User Login
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { 
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        private int _age = 18;
        /// <summary>
        /// Users age
        /// </summary>
        public int Age
        {
            get { return _age; }
            set { 
                _age = value;
            }
        }

        private AuthenticatedUser _buddyUser;
        /// <summary>
        /// buddy user item
        /// </summary>
        public AuthenticatedUser BuddyUser
        {
            get { return _buddyUser; }
            set { 
                _buddyUser = value;
                this.Age = this._buddyUser.Age;
                RaisePropertyChanged("BuddyUser");
            }
        }

        private string _email;
        /// <summary>
        /// user email
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { 
                _email = value;
                RaisePropertyChanged("Email");
            }
        }
        
        
    }
}
