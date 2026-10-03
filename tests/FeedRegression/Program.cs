using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Parse;
#if PHONE
using BitBankWP_places_app.Model;
using BitBankWP_places_app.ViewModel;
#else
using FriendsEyeAssist_w8.Model;
using FriendsEyeAssist_w8.ViewModel;
#endif
class Program {
 static int checks;
 static void Check(bool condition, string message) { checks++; if (!condition) throw new Exception(message); }
 static void Main() {
  var model = new MainViewModel();
#if PHONE
  model.MyCoordinate = new System.Device.Location.GeoCoordinate(1, 2);
#endif
  var active = new AssistsPhoto { ObjectId = "active" };
  model.PhotoItems.Add(active);
  ParseObject.Results = new[] { new ParseObject("AssistPhoto") { ObjectId = "near" } };
  model.LoadNearPhotos().GetAwaiter().GetResult();
  Check(model.PhotoItems.Count == 1 && model.PhotoItems[0] == active, "Nearby refresh must preserve active photos");
  Check(model.NearestPhotoItems.Count == 1 && model.NearestPhotoItems[0].ObjectId == "near", "Nearby result should populate nearby feed");
  Check(model.NearestImages.Count == 1, "Nearby photo should populate thumbnails");
  model.LoadNearPhotos().GetAwaiter().GetResult();
  Check(model.NearestPhotoItems.Count == 1, "Repeated nearby refresh must not duplicate photos");
  var nearby = model.NearestPhotoItems;
  var images = model.NearestImages;
  model.LoadSomePlaces().GetAwaiter().GetResult();
  Check(model.NearestPhotoItems == nearby && nearby.Count == 1, "Active refresh must preserve nearby feed");
  Check(model.NearestImages == images && images.Count == 1, "Active refresh must preserve nearby thumbnails");
  Check(model.PhotoItems.Count == 1 && model.PhotoItems[0].ObjectId == "near", "Active refresh should replace active feed");
  ParseObject.Results = new ParseObject[0];
  model.LoadNearPhotos().GetAwaiter().GetResult();
  Check(model.NearestPhotoItems.Count == 0 && model.NearestImages.Count == 0, "Empty nearby response must clear stale nearby results");
  Check(model.PhotoItems.Count == 1, "Empty nearby refresh must preserve active feed");
  model.Notifications.Clear();
  model.NearestPhotoItems = new ObservableCollection<AssistsPhoto>();
  Check(model.Notifications.Contains("NearestPhotoItems"), "Nearest setter must notify its own property");
  Check(!model.Notifications.Contains("PhotoItems"), "Nearest setter must not notify the active feed");
#if !PHONE
  Check(model.Notifications.Contains("LastNearestPhotoItems"), "Nearest setter must notify derived preview");
  ParseObject.Results = new[] { new ParseObject("AssistPhoto") };
  model.Notifications.Clear();
  model.LoadNearPhotos().GetAwaiter().GetResult();
  Check(model.LastNearestPhotoItems.Count == 1 && model.Notifications[model.Notifications.Count - 1] == "LastNearestPhotoItems", "Nearby preview must be notified after population");
  model.Notifications.Clear();
  model.LoadSomePlaces().GetAwaiter().GetResult();
  Check(model.LastPhotoItems.Count == 1 && model.Notifications[model.Notifications.Count - 1] == "LastPhotoItems", "Active preview must be notified after population");
#endif
  Console.WriteLine("PASS: " + checks + " feed regression assertions");
 }
}
