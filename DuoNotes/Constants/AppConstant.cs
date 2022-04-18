
using System.Globalization;

namespace DuoNotes.Constants {

    public class AppConstant {

        //syncfusion library key

        public const string KEY = "NjE3NTA2QDMyMzAyZTMxMmUzME5IeE9WK1BzQW5sODB6WGVqRThXNVdPdUV6Q2V1ZEtLL0h3UEorc1o5QVE9";


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
        public const string ConectionString = "DefaultEndpointsProtocol=https;AccountName=notesbucket;AccountKey=kCUWArA7EFfu2Zwt4R+zSC619apF409MZsLt39V04YAdKt4o+1ZRrZLLWh0mKbGIJ4PjkfcEHKweACWgcf4LcA==;EndpointSuffix=core.windows.net";

        //Azure Computer vision

        public const string ComputerVisionEndPoint = "https://duocmputervision.cognitiveservices.azure.com/";
        public const string ComputerVisionKey = "fad89e0cfd114521816a5ab0f5efa5ba";

        //Edit Notebooks and Notes

        public const string EditNote = "EditNote";
        public const string EditNotebook = "EditNotebook";

        // We need the Id of the notebook, when we edit a Note

        public const string NotebookId = "NotebookId";

        // Text and Html text

        public const string Text = "Text";
        public const string HtmlText = "HtmlText";

        //Note name

        public const string NoteName = "NoteName";

        //SfButtonStyle

        public const string EdtorToolBarBtonsstyle = "EdtorToolBarBtons";
    }
}
