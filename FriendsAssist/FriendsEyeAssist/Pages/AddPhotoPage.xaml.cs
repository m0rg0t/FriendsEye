using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BitBankWP_places_app.ViewModel;
using BitBankWP_places_app.Model;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Windows.Devices.Geolocation;
using System.Device.Location;

namespace BitBankWP_places_app.Pages
{
    public partial class AddPhotoPage : PhoneApplicationPage
    {
        // Constructor
        public AddPhotoPage()
        {
            InitializeComponent();            
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        CameraCaptureTask cameraCaptureTask;

        private void CancelAppBarButton_Click(object sender, EventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.NewPhoto = new AssistsPhoto();
                this.NavigationService.GoBack();
            }
            catch { };
        }

        /// <summary>
        /// 
        /// </summary>
        public GeoCoordinate MyCoordinate { 
            get; set; 
        }

        private double _accuracy = 0.0;

        /// <summary>
        /// 
        /// </summary>
        private async void GetCurrentCoordinate()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;

            try
            {
                Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                                   TimeSpan.FromSeconds(10));
                _accuracy = currentPosition.Coordinate.Accuracy;
                Dispatcher.BeginInvoke(() =>
                {
                    MyCoordinate = new GeoCoordinate(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
                });
            }
            catch (Exception ex)
            {
                // Couldn't get current location - location might be disabled in settings
                MessageBox.Show("Current location cannot be obtained. Check that location service is turned on in phone settings.");
            }
        }

        private async void AddAppBarButton_Click(object sender, EventArgs e)
        {
            this.BusyBar.IsRunning = true;
            try
            {
                ViewModelLocator.MainStatic.NewPhoto.ImageSource = bmp;
                ViewModelLocator.MainStatic.NewPhoto.Lat = MyCoordinate.Latitude;
                ViewModelLocator.MainStatic.NewPhoto.Lon = MyCoordinate.Longitude;
                await ViewModelLocator.MainStatic.SaveItemToParse(ViewModelLocator.MainStatic.NewPhoto);
                this.BusyBar.IsRunning = false;
                this.NavigationService.GoBack();
            }
            catch {
                this.BusyBar.IsRunning = false;
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cameraCaptureTask = new CameraCaptureTask();
                cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);

                GetCurrentCoordinate();
            }
            catch { };
        }

        public WriteableBitmap bmp { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                //MessageBox.Show(e.ChosenPhoto.Length.ToString());

                //Code to display the photo on the page in an image control named myImage.
                var bmpi = new BitmapImage();
                bmpi.SetSource(e.ChosenPhoto);
                bmp = new WriteableBitmap(bmpi);
                //bmp
                myImage.Source = bmpi;
            }
        }

        private void MakePhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cameraCaptureTask.Show();
            }
            catch { };
        }

        private void myImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                cameraCaptureTask.Show();
            }
            catch { };
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}