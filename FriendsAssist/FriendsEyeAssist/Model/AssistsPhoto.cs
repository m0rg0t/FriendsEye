using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using System.Windows.Media.Imaging;
using Parse;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BitBankWP_places_app.Model
{
    public class AssistsPhoto: ViewModelBase
    {
        public AssistsPhoto()
        {
        }

        private string _objectId;
        /// <summary>
        /// Идентификатор места
        /// </summary>
        public string ObjectId
        {
            get { return _objectId; }
            set { 
                _objectId = value;               
                RaisePropertyChanged("ObjectId");
            }
        }
        

        private string _comment;
        /// <summary>
        /// Comment for item
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set {
                _comment = value;
                RaisePropertyChanged("Comment");
            }
        }

        private string _title;
        /// <summary>
        /// Название
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { 
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private DateTime _createdAt;
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { 
                _createdAt = value;
                RaisePropertyChanged("CreatedAt");
                RaisePropertyChanged("CreatedString");
            }
        }

        private string _createdString;
        /// <summary>
        /// When photo was created
        /// </summary>
        public string CreatedString
        {
            get {
                _createdString = this.CreatedAt.ToString();
                return _createdString; 
            }
            set { 
                _createdString = value;
                RaisePropertyChanged("CreatedString");
            }
        }
        

        private DateTime _updatedAt;
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdatedAt
        {
            get { return _updatedAt; }
            set { 
                _updatedAt = value;
                RaisePropertyChanged("UpdatedAt");
            }
        }

        private double _lat;
        /// <summary>
        /// Широта
        /// </summary>
        public double Lat
        {
            get { return _lat; }
            set { 
                _lat = value;
                RaisePropertyChanged("Lat");
                RaisePropertyChanged("GeoPoint");
            }
        }


        private double _lon;
        /// <summary>
        /// Долгота
        /// </summary>
        public double Lon
        {
            get { return _lon; }
            set { 
                _lon = value;
                RaisePropertyChanged("Lon");
                RaisePropertyChanged("GeoPoint");
            }
        }

        private string _image;
        /// <summary>
        /// 
        /// </summary>
        public string Image
        {
            get { return _image; }
            set { 
                _image = value;
                RaisePropertyChanged("Image");
            }
        }

        private string _userId;
        /// <summary>
        /// Идентификатор пользователя, добавившего место
        /// </summary>
        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        private GeoCoordinate _position;
        /// <summary>
        /// 
        /// </summary>
        public GeoCoordinate Position
        {
            get {
                _position = new GeoCoordinate(this.Lat, this.Lon);
                return _position; 
            }
            private set {}
        }

        private WriteableBitmap _imageSource;
        /// <summary>
        /// 
        /// </summary>
        public WriteableBitmap ImageSource
        {
            get { return _imageSource; }
            set { 
                _imageSource = value;
                RaisePropertyChanged("ImageSource");
            }
        }

        private ParseGeoPoint _geoPoint = new ParseGeoPoint();
        /// <summary>
        /// 
        /// </summary>
        public ParseGeoPoint GeoPoint
        {
            get {
                ParseGeoPoint _geoPoint = new ParseGeoPoint(this.Lat, this.Lon);
                return _geoPoint;
            }
            set { 
                _geoPoint = value;
                RaisePropertyChanged("GeoPoint");
            }
        }

        private List<AssistAnswer> _AssistsAnswersItems;
        /// <summary>
        /// Комментарии к записи
        /// </summary>
        public List<AssistAnswer> AssistsAnswersItems
        {
            get { return _AssistsAnswersItems; }
            set
            {
                _AssistsAnswersItems = value;
                RaisePropertyChanged("AssistsAnswersItems");
            }
        }

        public async Task<bool> LoadAnswers()
        {
            try
            {
                // Create a query for places
                var query = from comment in ParseObject.GetQuery("AssistAnswer")
                            where comment.Get<string>("photoId") == this.ObjectId.ToString()
                            select comment;
                query = query.Limit(80);
                IEnumerable<ParseObject> results = await query.FindAsync();
                this.AssistsAnswersItems = new List<AssistAnswer>();
                var answersList = new List<AssistAnswer>();
                foreach (var item in results)
                {
                    try
                    {
                        //AddPlaceFromParseObject(item, "search");
                        var cItem = new AssistAnswer();
                        cItem.UserId = item.Get<string>("userId");
                        cItem.Comment = item.Get<string>("comment");
                        cItem.UserName = item.Get<string>("userName");
                        cItem.UserImage = item.Get<string>("userImage");
                        cItem.PhotoId = item.Get<string>("photoId");
                        cItem.ObjectId = item.ObjectId.ToString();
                        cItem.CreatedDate = item.CreatedAt.Value;
                        answersList.Add(cItem);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    };
                };
                this.AssistsAnswersItems = answersList;
            }
            catch { };
            return true;
        }

    }
}
