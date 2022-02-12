using DuoNotes.Constants;
using DuoNotes.Model;
using DuoNotes.Services;
using DuoNotes.View;

using Syncfusion.Licensing;

using System.Collections.ObjectModel;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes {
    public partial class App : Application {

        public static FirebaseServices FirebaseServices { get; set; }

        public App() {

            FirebaseServices = new FirebaseServices();

            SyncfusionLicenseProvider.RegisterLicense(AppConstant.KEY);

            InitializeComponent();

            var UserId = Preferences.Get(AppConstant.UserID, string.Empty);

            if (!string.IsNullOrEmpty(UserId)) {
                MainPage = new NavigationPage(new MainPage());
            } else {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
