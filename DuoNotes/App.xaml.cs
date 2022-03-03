using DuoNotes.Constants;
using DuoNotes.Pages;
using DuoNotes.Services;
using DuoNotes.View;

using Syncfusion.Licensing;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes {
    public partial class App : Application {

        public static FirebaseServices FirebaseServices { get; set; }

        public static AzureServices AzureServices { get; set; }

        public App() {

            FirebaseServices = new FirebaseServices();

            AzureServices = new AzureServices();

            SyncfusionLicenseProvider.RegisterLicense(AppConstant.KEY);

            InitializeComponent();

            MainPage = new NavigationPage(new SplashScreenPage());
           
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
