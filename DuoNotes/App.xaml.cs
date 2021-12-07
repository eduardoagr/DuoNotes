﻿using DuoNotes.Services;
using DuoNotes.View;

using Syncfusion.Licensing;

using System.Globalization;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DuoNotes {
    public partial class App : Application {

        private const string KEY = "NTEzNDUzQDMxMzkyZTMzMmUzMEFHQ1hQZVBJa1ppRWFJYk1QU0xrbDF6bTNTVjFRNTV3dTgvZEVZK3ByTW89";

        public const string WEB_API_KEY = "AIzaSyAxdD4aXTmGRN-BwLX4ItYusIc35r4_VVQ";

        public const string UID = "UID";

        public static string languages = CultureInfo.CurrentCulture.Name;

        public static string UserID = string.Empty;

        public static FirebaseServices services;

        public const string FirebaseToken = "FirebaseToken";

        public App() {

            SyncfusionLicenseProvider.RegisterLicense(KEY);

            InitializeComponent();

            UserID = Preferences.Get(UID, string.Empty);

            System.Console.WriteLine(App.UserID);

            if (!string.IsNullOrEmpty(UserID)) {
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
