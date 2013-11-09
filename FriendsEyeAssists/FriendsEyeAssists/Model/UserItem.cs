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
        
    }
}
