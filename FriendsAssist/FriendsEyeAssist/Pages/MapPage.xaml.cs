using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using Microsoft.Xna.Framework;
using Microsoft.Phone.Maps.Controls;
using BitBankWP_places_app.ViewModel;
using System.Windows.Media;
using BitBankWP_places_app.Controls;
using BitBankWP_places_app.Model;
using System.Windows.Input;

namespace BitBankWP_places_app.Pages
{
    public partial class MapPage : PhoneApplicationPage
    {
        // Constructor
        public MapPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void DrawMapMarkers()
        {
            PlaceMap.Layers.Clear();
            MapLayer mapLayer = new MapLayer();

            // Draw marker for current position
            if (ViewModelLocator.MainStatic.PhotoItems.Count()>0)
            {
                foreach (var item in ViewModelLocator.MainStatic.PhotoItems)
                {
                    DrawMapMarker(item.Position, item, mapLayer);
                };                
            }

            PlaceMap.Layers.Add(mapLayer);
            //PlaceMap.Center = ViewModelLocator.MainStatic.PhotoItems.FirstOrDefault().Position;
            try
            {
                PlaceMap.Center = ViewModelLocator.MainStatic.MyCoordinate;
            }
            catch {
                try
                {
                    PlaceMap.Center = ViewModelLocator.MainStatic.PhotoItems.FirstOrDefault().Position;
                }
                catch { };
            };
        }

        private void DrawMapMarker(GeoCoordinate coordinate, AssistsPhoto place, MapLayer mapLayer)
        {
            // Create a map marker
            var item = new MapItemControl();

            item.Image = place.Image;
            item.Tag = place.ObjectId; //new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);

            // Enable marker to be tapped for location information            
            item.MouseLeftButtonUp += new MouseButtonEventHandler(Marker_Click);

            // Create a MapOverlay and add marker
            MapOverlay overlay = new MapOverlay();
            overlay.Content = item; //polygon;
            overlay.GeoCoordinate = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
            overlay.PositionOrigin = new System.Windows.Point(0.5, 1.0);
            mapLayer.Add(overlay);
        }

        private void Marker_Click(object sender, MouseButtonEventArgs e)
        {
            try {
                ViewModelLocator.MainStatic.CurrentItem = ViewModelLocator.MainStatic.PhotoItems.FirstOrDefault(c=>c.ObjectId==(sender as MapItemControl).Tag.ToString());
                this.NavigationService.Navigate(new Uri("/Pages/ViewPhotoPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void PlaceMap_Loaded(object sender, RoutedEventArgs e)
        {
            DrawMapMarkers();
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