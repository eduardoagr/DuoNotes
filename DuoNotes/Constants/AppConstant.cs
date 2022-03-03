
using System.Globalization;

namespace DuoNotes.Constants {
    public class AppConstant {

        //syncfusion library key

        public const string KEY = "NTY0Njk3QDMxMzkyZTM0MmUzMEVJRzNEYmRZQnZhdjIyeXRDY3JpMXgwUUg1MnBoQ1AxMWFYZlF6Z2dIVEE9";

        
        //When we save in our database, we want to know the language of the device, so we can adjust, the date
        
        public static string languages = CultureInfo.CurrentCulture.Name;

        
        // User ID in firebase

        public static string UserID = "UserID";


        //Firebase keys and tokens

        public const string WEB_API_KEY = "AIzaSyAxdD4aXTmGRN-BwLX4ItYusIc35r4_VVQ";

        public const string FirebaseRefreshToken = "FirebaseRefreshToken";

        public const string FirebaseToken = "FirebaseToken";



        //Firebase child name

        public const string Notebooks = "Notebooks";

        public const string Notes = "Notes";


        //We need the ID to query the database for the selected notebook
        
        public const string SelectedNotebook = "SelectedNotebook";


        public const string SelectedNote = "SelectedNote";

        //Azure Blob storage

        public const string ContanerName = "notes";
        public const string ConectionString = "DefaultEndpointsProtocol=https;AccountName=notesbucket;AccountKey=YnF1cUnsJnmco2pAMmppa9Af2TKQLAkCxUxmyhfmbmg7inyO5LT0XMAhm+dFdLcyII+BNHWWGJaI6Exs0OVzwg==;EndpointSuffix=core.windows.net";
    }
}
