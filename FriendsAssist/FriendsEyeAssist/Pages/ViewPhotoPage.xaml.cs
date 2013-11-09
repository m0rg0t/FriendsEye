using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using BitBankWP_places_app.ViewModel;
using System.Device.Location;
using Windows.Devices.Geolocation;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Phone.Maps.Services;
using BitBankWP_places_app.Controls;

namespace BitBankWP_places_app.Pages
{
    public partial class ViewPhotoPage : PhoneApplicationPage
    {
        // Constructor
        public ViewPhotoPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void PlaceMap_Loaded(object sender, RoutedEventArgs e)
        {
            Map item = new Map();
            //item.Center
            try
            {                
                LoadingComments();
                /*PlaceMap.Center.Latitude = ViewModelLocator.MainStatic.CurrentItem.Lon;
                PlaceMap.Center.Longitude = ViewModelLocator.MainStatic.CurrentItem.Lat;
                var i = PlaceMap.Center;
                var b = i;*/

                //GetCurrentCoordinate();
                DrawMapMarkers();
            }
            catch { };
        }

        public async void LoadingComments() {
            try
            {
                await ViewModelLocator.MainStatic.CurrentItem.LoadAnswers();
                //this.CommentsList.ItemsSource = ViewModelLocator.MainStatic.CurrentItem.CommentItems;
            }
            catch { };
        }

        private GeoCoordinate MyCoordinate = null;
        private ReverseGeocodeQuery MyReverseGeocodeQuery = null;

       private void DrawMapMarkers()
        {
            PlaceMap.Layers.Clear();
            MapLayer mapLayer = new MapLayer();
         
            // Draw marker for current position
            if (ViewModelLocator.MainStatic.CurrentItem.Position != null)
            {
                try
                {
                    //DrawAccuracyRadius(mapLayer);
                }
                catch { };
                DrawMapMarker(ViewModelLocator.MainStatic.CurrentItem.Position, Colors.Red, mapLayer);
            }

            PlaceMap.Layers.Add(mapLayer);
        }

        private void DrawMapMarker(GeoCoordinate coordinate, Color color, MapLayer mapLayer)
        {
            // Create a map marker
            var item = new MapItemControl();
            Polygon polygon = new Polygon();
            polygon.Points.Add(new Point(0, 0));
            polygon.Points.Add(new Point(0, 75));
            polygon.Points.Add(new Point(25, 0));
            polygon.Fill = new SolidColorBrush(color);

            item.Image = ViewModelLocator.MainStatic.CurrentItem.Image;
            item.Tag = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);

            // Enable marker to be tapped for location information
            polygon.Tag = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
            polygon.MouseLeftButtonUp += new MouseButtonEventHandler(Marker_Click);

            // Create a MapOverlay and add marker
            MapOverlay overlay = new MapOverlay();
            overlay.Content = item; //polygon;
            overlay.GeoCoordinate = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
            overlay.PositionOrigin = new Point(0.5, 1.0);
            mapLayer.Add(overlay);
        }
        
    
        private void Marker_Click(object sender, EventArgs e)
        {
            Polygon p = (Polygon)sender;
            GeoCoordinate geoCoordinate = (GeoCoordinate)p.Tag;
            MyReverseGeocodeQuery = new ReverseGeocodeQuery();
            MyReverseGeocodeQuery.GeoCoordinate = new GeoCoordinate(geoCoordinate.Latitude, geoCoordinate.Longitude);
            MyReverseGeocodeQuery.QueryCompleted += ReverseGeocodeQuery_QueryCompleted;
            MyReverseGeocodeQuery.QueryAsync();
        }
    
        private void ReverseGeocodeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                if (e.Result.Count > 0)
                {
                    MapAddress address = e.Result[0].Information.Address;
                    String msgBoxText = "";

                    if (address.Country.Length > 0) msgBoxText += "\n" + address.Country;
                    MessageBox.Show(msgBoxText, "Map Explorer", MessageBoxButton.OK);
                } 
            }
        }

        private double _accuracy = 0.0;    
    
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
     
        private void DrawAccuracyRadius(MapLayer mapLayer)
        {
            // The ground resolution (in meters per pixel) varies depending on the level of detail
            // and the latitude at which it’s measured. It can be calculated as follows:
            double metersPerPixels = (Math.Cos(MyCoordinate.Latitude * Math.PI / 180) * 2 * Math.PI * 6378137) / (256 * Math.Pow(2, PlaceMap.ZoomLevel));
            double radius = _accuracy / metersPerPixels;

            Ellipse ellipse = new Ellipse();
            ellipse.Width = radius * 2;
            ellipse.Height = radius * 2;
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(75, 200, 0, 0));

            MapOverlay overlay = new MapOverlay();
            overlay.Content = ellipse;
            overlay.GeoCoordinate = new GeoCoordinate(MyCoordinate.Latitude, MyCoordinate.Longitude);
            overlay.PositionOrigin = new Point(0.5, 0.5);
            mapLayer.Add(overlay);
        }

        private void AppBarCommentButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewModelLocator.MainStatic.User.IsLogged)
                {
                    this.NavigationService.Navigate(new Uri("/Pages/AddCommentPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Для добавления отзыва вы должны авторизоваться.");
                    this.NavigationService.Navigate(new Uri("/Pages/FacebookLoginPage.xaml", UriKind.Relative));
                };
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