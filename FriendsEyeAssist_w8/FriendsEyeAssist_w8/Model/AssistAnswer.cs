using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitBankWP_places_app.Model
{
    public class AssistAnswer: ViewModelBase
    {
        public AssistAnswer()
        {
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

        private string _comment;
        /// <summary>
        /// 
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set { 
                _comment = value;
                RaisePropertyChanged("Comment");
            }
        }

        private string _userId;
        /// <summary>
        /// Идентификатор пользователя, добавившего комментарий
        /// </summary>
        public string UserId
        {
            get { return _userId; }
            set { 
                _userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        private string _photoId;
        /// <summary>
        /// 
        /// </summary>
        public string PhotoId
        {
            get { return _photoId; }
            set { 
                _photoId = value;
                RaisePropertyChanged("PhotoId");
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

        private string _userName;
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { 
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        private DateTime _createdDate;
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { 
                _createdDate = value;
                RaisePropertyChanged("CreatedDate");
                RaisePropertyChanged("DateString");
            }
        }
        

        public string DateString
        {
            get { return CreatedDate.ToString(); }
            set {}
        }
        
        
        
    }
}
