using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace BitBankWP_places_app.Controls
{
    public partial class MapItemControl : UserControl
    {
        public MapItemControl()
        {
            InitializeComponent();
        }

        private string _image = "/Assets/grumpy-cat-8141_preview_zps9177ab07.png";
        /// <summary>
        /// 
        /// </summary>
        public string Image
        {
            get { return _image; }
            set { 
                _image = value;
            }
        }
        
    }
}
