using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FriendsEyeAssists.ViewModel
{
    public class UserViewModel: ViewModelBase
    {
        public UserViewModel()
        {
        }

        private string _facebookId;
        /// <summary>
        /// 
        /// </summary>
        public string FacebookId
        {
            get { return _facebookId; }
            set { 
                _facebookId = value;
                RaisePropertyChanged("FacebookId");
            }
        }

        private bool _isLogged;
        /// <summary>
        /// 
        /// </summary>
        public bool IsLogged
        {
            get { return _isLogged; }
            set { 
                _isLogged = value;
                if (_isLogged)
                {                   
                    LoggedInVisibility = Visibility.Visible;
                    UnLoggedVisibility = Visibility.Collapsed;
                } else {
                    LoggedInVisibility = Visibility.Collapsed;
                    UnLoggedVisibility = Visibility.Visible;
                };
                RaisePropertyChanged("IsLogged");
            }
        }

        private Visibility _loggedInVisibility = Visibility.Collapsed;
        /// <summary>
        /// 
        /// </summary>
        public Visibility LoggedInVisibility
        {
            get { return _loggedInVisibility; }
            set { 
                _loggedInVisibility = value;
                RaisePropertyChanged("LoggedInVisibility");
            }
        }

        private Visibility _UnloggedVisibility = Visibility.Visible;
        /// <summary>
        /// 
        /// </summary>
        public Visibility UnLoggedVisibility
        {
            get { return _UnloggedVisibility; }
            set
            {
                _UnloggedVisibility = value;
                RaisePropertyChanged("UnLoggedVisibility");
            }
        }
        

        private string _facebookToken = "";
        /// <summary>
        /// 
        /// </summary>
        public string FacebookToken
        {
            get { return _facebookToken; }
            set { 
                _facebookToken = value;
                RaisePropertyChanged("FacebookToken");
            }
        }

        private string _username;
        /// <summary>
        /// 
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { 
                _username = value;
                RaisePropertyChanged("Username");
            }
        }

        private string _firstName;
        /// <summary>
        /// 
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { 
                _firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        private string _lastName;
        /// <summary>
        /// 
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set { 
                _lastName = value;
                RaisePropertyChanged("LastName");
            }
        }      
        

        private string _userImage;
        /// <summary>
        /// 
        /// </summary>
        public string UserImage
        {
            get { return _userImage; }
            set { 
                _userImage = value;
                RaisePropertyChanged("UserImage");
            }
        }

        private string _objectId;
        /// <summary>
        /// 
        /// </summary>
        public string ObjectId
        {
            get { return _objectId; }
            set { 
                _objectId = value;
                RaisePropertyChanged("ObjectId");
            }
        }
        
        
        
    }
}
