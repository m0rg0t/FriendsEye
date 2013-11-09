using Buddy;
using FriendsEyeAssists.Model;
using FriendsEyePhoneLibrary.BuddyViewModel;
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
            BuddyItem = new BuddyViewModel();
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
        

        private BuddyViewModel _buddyItem;
        /// <summary>
        /// 
        /// </summary>
        public BuddyViewModel BuddyItem
        {
            get { return _buddyItem; }
            set { 
                _buddyItem = value;
                RaisePropertyChanged("BuddyItem");
            }
        }
        


    }
}