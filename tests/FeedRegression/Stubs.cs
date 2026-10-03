// Offline test doubles for unavailable Windows/Phone and retired Parse services.
// Collection mutation and property notifications execute in the linked production ViewModel.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace GalaSoft.MvvmLight {
 public class ViewModelBase {
  public readonly List<string> Notifications = new List<string>();
  protected void RaisePropertyChanged(string name) { Notifications.Add(name); }
 }
}
namespace Parse {
 public struct ParseGeoPoint { public ParseGeoPoint(double lat, double lon) {} }
 public class ParseFile {
  public Uri Url = new Uri("https://example.invalid/photo.jpg");
  public ParseFile(string name, byte[] data) {} public Task SaveAsync() { return Task.FromResult(true); }
 }
 public class ParseObject {
  public static IEnumerable<ParseObject> Results = new List<ParseObject>();
  public string ObjectId = "photo";
  public DateTime? CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow;
  public ParseObject(string name) {}
  public static ParseQuery GetQuery(string name) { return new ParseQuery(); }
  public object this[string key] { set {} }
  public T Get<T>(string key) {
   object value;
   if (typeof(T) == typeof(string)) value = "fixture";
   else if (typeof(T) == typeof(double)) value = 1.0;
   else if (typeof(T) == typeof(int)) value = 0;
   else value = new ParseFile("photo", new byte[0]);
   return (T)value;
  }
  public Task SaveAsync() { return Task.FromResult(true); }
 }
 public class ParseQuery {
  public ParseQuery WhereNear(string field, ParseGeoPoint point) { return this; }
  public ParseQuery Limit(int count) { return this; }
  public ParseQuery Where(Func<ParseObject, bool> predicate) { return this; }
  public ParseQuery Select(Func<ParseObject, ParseObject> selector) { return this; }
  public Task<IEnumerable<ParseObject>> FindAsync() { return Task.FromResult(ParseObject.Results); }
 }
}
namespace Windows.Devices.Geolocation {
 public enum PositionAccuracy { High }
 public class Coordinate { public double Accuracy, Latitude, Longitude; }
 public class Geoposition { public Coordinate Coordinate = new Coordinate(); }
 public class Geolocator {
  public PositionAccuracy DesiredAccuracy;
  public Task<Geoposition> GetGeopositionAsync() { return Task.FromResult(new Geoposition()); }
  public Task<Geoposition> GetGeopositionAsync(TimeSpan age, TimeSpan timeout) { return GetGeopositionAsync(); }
 }
}
namespace System.Device.Location {
 public class GeoCoordinate { public double Latitude, Longitude; public GeoCoordinate(double lat, double lon) { Latitude = lat; Longitude = lon; } }
}
namespace System.Windows { public class Placeholder {} }
namespace System.Windows.Threading { public class Placeholder {} }
namespace System.Windows.Media.Imaging {
 public class WriteableBitmap { public int PixelWidth, PixelHeight; }
 public static class Extensions { public static void SaveJpeg(WriteableBitmap bitmap, System.IO.Stream stream, int width, int height, int rotation, int quality) {} }
}
namespace Windows.UI.Xaml { public enum Visibility { Visible, Collapsed } }
#if PHONE
namespace BitBankWP_places_app.Model
#else
namespace FriendsEyeAssist_w8.Model
#endif
{
 public class AssistAnswer { public string Comment, PhotoId; }
 public class AssistsPhoto {
  public string ObjectId, Title, Comment, UserId, Image;
  public double Lat, Lon; public int Category; public DateTime CreatedAt, UpdatedAt;
  public System.Windows.Media.Imaging.WriteableBitmap ImageSource;
  public List<AssistAnswer> AssistsAnswersItems = new List<AssistAnswer>();
 }
}
#if PHONE
namespace BitBankWP_places_app.ViewModel
#else
namespace FriendsEyeAssist_w8.ViewModel
#endif
{
 public class UserViewModel { public string UserImage, Username, ObjectId; }
 public class ViewModelLocator { public static MainViewModel MainStatic = new MainViewModel(); }
}
