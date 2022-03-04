using DuoNotes.Constants;
using DuoNotes.Pages;
using DuoNotes.Services;

using Syncfusion.Licensing;

using Xamarin.Forms;

namespace DuoNotes {
    public partial class App : Application {

        public static FirebaseService FirebaseService { get; set; }

        public static AzureService AzureService { get; set; }

        public App() {

            AzureService = new AzureService();

            FirebaseService = new FirebaseService();

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
