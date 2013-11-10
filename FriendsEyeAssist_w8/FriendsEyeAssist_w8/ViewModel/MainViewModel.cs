using FriendsEyeAssist_w8.Model;
using GalaSoft.MvvmLight;
using Parse;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Xaml;
using System.Net.Http;
using Windows.Devices.Geolocation;
using System;

namespace FriendsEyeAssist_w8.ViewModel
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

        private UserViewModel _user = new UserViewModel();
        /// <summary>
        /// 
        /// </summary>
        public UserViewModel User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
            }
        }

        private bool _loading;
        /// <summary>
        /// загрузка
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

        public bool IsLoaded
        {
            private set
            {
            }
            get
            {
                return !Loading;
            }
        }

        public double Lat = 55.758;
        public double Lon = 37.611;

        public async Task<bool> GetCurrentPosition()
        {
            try
            {
                var geolocator = new Geolocator();
                Geoposition position = await geolocator.GetGeopositionAsync();
                var str = position.ToString();
                Lat = position.Coordinate.Latitude;
                Lon = position.Coordinate.Longitude;
                //await GetPlaceInfo(Lat, Lon);
            }
            catch
            {
                //GetPlaceInfo(Lat, Lon);
            };
            return true;
        }

        public async Task<string> MakeWebRequest(string url = "")
        {
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility LoadingVisibility
        {
            get
            {
                if (IsLoaded)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                };
            }
            private set
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoadNearPhotos()
        {
            try
            {
                // User's location
                var userGeoPoint = new ParseGeoPoint(this.Lat, this.Lon); //MyCoordinate.Latitude, MyCoordinate.Longitude
                // Create a query for places
                var query = ParseObject.GetQuery("AssistPhoto");
                //Interested in locations near user.
                query = query.WhereNear("latlon", userGeoPoint);
                // Limit what could be a lot of points.
                query = query.Limit(80);

                IEnumerable<ParseObject> results = await query.FindAsync();

                PhotoItems = new ObservableCollection<AssistsPhoto>();
                NearestImages = new Collection<string>();
                foreach (var item in results)
                {
                    AddPlaceFromParseObject(item);
                };

                /*if (PhotoItems.Count < 1)
                {
                    await LoadSomePlaces();
                };*/
            }
            catch { };
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void AddPlaceFromParseObject(ParseObject item, string type = "nearest")
        {
            try
            {
                var PhotoItem = new AssistsPhoto();
                //PhotoItem.Title = item.Get<string>("title");
                PhotoItem.Category = item.Get<int>("category");
                PhotoItem.Comment = item.Get<string>("comment");
                PhotoItem.Lat = item.Get<double>("lat");
                PhotoItem.Lon = item.Get<double>("lon");
                PhotoItem.ObjectId = item.ObjectId.ToString();
                try
                {
                    var file = item.Get<ParseFile>("photo");
                    PhotoItem.Image = file.Url.ToString();
                    if (type == "nearest")
                    {
                        NearestImages.Add(file.Url.ToString());
                    };
                }
                catch { };
                if (type == "nearest")
                {
                    //SearchPlaceItems.Add(PhotoItem);
                    NearestPhotoItems.Add(PhotoItem);
                }
                else
                {
                    PhotoItems.Add(PhotoItem);
                };
            }
            catch { };
        }

        public async Task<bool> LoadSomePlaces()
        {
            try
            {
                //var query = ParseObject.GetQuery("AssistPhoto");
                var query = from place in ParseObject.GetQuery("AssistPhoto")
                            where (place.Get<int>("category") == 0)
                            select place;
                //var query = from item in ParseObject.GetQuery("Place") select item;
                query = query.Limit(80);
                IEnumerable<ParseObject> results = await query.FindAsync();

                PhotoItems = new ObservableCollection<AssistsPhoto>();
                NearestImages = new Collection<string>();
                foreach (var item in results)
                {
                    AddPlaceFromParseObject(item, "active");
                };
            }
            catch { };
            return true;
        }

        private Collection<string> _nearestImages = new Collection<string>();
        /// <summary>
        /// 
        /// </summary>
        public Collection<string> NearestImages
        {
            get { return _nearestImages; }
            set
            {
                _nearestImages = value;
                RaisePropertyChanged("NearestImages");
            }
        }

        private AssistAnswer _newComment = new AssistAnswer();
        /// <summary>
        /// 
        /// </summary>
        public AssistAnswer NewComment
        {
            get { return _newComment; }
            set
            {
                _newComment = value;
                RaisePropertyChanged("NewComment");
            }
        }

        public ObservableCollection<AssistsPhoto> LastPhotoItems
        {
            set { }
            get
            {
                ObservableCollection<AssistsPhoto> items = new ObservableCollection<AssistsPhoto>();
                try
                {
                    items = new ObservableCollection<AssistsPhoto>(this.PhotoItems.Take(2));
                }
                catch
                {
                };
                return items;
            }
        }


        private ObservableCollection<AssistsPhoto> _photoItems = new ObservableCollection<AssistsPhoto>();
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<AssistsPhoto> PhotoItems
        {
            get { return _photoItems; }
            set
            {
                _photoItems = value;
                RaisePropertyChanged("PhotoItems");
                RaisePropertyChanged("LastPhotoItems");
            }
        }

        public ObservableCollection<AssistsPhoto> LastNearestPhotoItems
        {
            set { }
            get
            {
                ObservableCollection<AssistsPhoto> items = new ObservableCollection<AssistsPhoto>();
                try
                {
                    items = new ObservableCollection<AssistsPhoto>(this.NearestPhotoItems.Take(2));
                }
                catch {                    
                };
                return items;
            }
        }

        private ObservableCollection<AssistsPhoto> _nearestPlaceItems = new ObservableCollection<AssistsPhoto>();
        /// <summary>
        /// Nearest places
        /// </summary>
        public ObservableCollection<AssistsPhoto> NearestPhotoItems
        {
            get { return _nearestPlaceItems; }
            set
            {
                _nearestPlaceItems = value;
                RaisePropertyChanged("PhotoItems");
            }
        }




        private AssistsPhoto _currentItem = new AssistsPhoto();
        /// <summary>
        /// Текущее место
        /// </summary>
        public AssistsPhoto CurrentItem
        {
            get { return _currentItem; }
            set
            {
                _currentItem = value;
                RaisePropertyChanged("CurrentItem");
            }
        }

        public async Task<bool> LoadData()
        {
            this.Loading = true;
            
            try
            {
                await GetCurrentPosition();
                await LoadNearPhotos();
                await LoadSomePlaces();
                RaisePropertyChanged("LastPhotoItems");
                RaisePropertyChanged("LastNearestPhotoItems");
            }
            catch { };
            this.Loading = false;
            return true;
        }    

    }
}