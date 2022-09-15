
using Acr.UserDialogs;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;

using FFImageLoading.Forms.Platform;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace DuoNotes.Droid
{

    [Activity(Label = "XamNotes", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait,
     ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
   | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]

    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            Forms.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            FormsMaterial.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);

            LoadApplication(new App());

            Window.SetSoftInputMode(SoftInput.AdjustResize);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}