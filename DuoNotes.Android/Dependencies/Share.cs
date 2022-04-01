using Android.Content;

using AndroidX.Core.Content;

using DuoNotes.Droid.Dependencies;
using DuoNotes.Interfaces;

using Java.IO;

using System.Threading.Tasks;

using Xamarin.Forms;

[assembly: Dependency(typeof(Share))]
namespace DuoNotes.Droid.Dependencies {
    public class Share : IShare {
        public Task Show(string title, string messge, string filePath, string ext) {

            var intent = new Intent(Intent.ActionSend);
            if (ext.Equals(".pdf")) {
                intent.SetType("application/pdf");
            } else if (ext.Equals(".docx")) {
                intent.SetType("application/msword");
            } else {
                intent.SetType("text/plain");
            }
            var documentUri = FileProvider.GetUriForFile(Android.App.Application.Context,
                "com.companyname.XamNotes.provider",
                new File(filePath));

            intent.PutExtra(Intent.ExtraText, title);
            intent.PutExtra(Intent.ExtraSubject, messge);
            intent.PutExtra(Intent.ExtraStream, documentUri);

            var chooserIntent = Intent.CreateChooser(intent, title);
            chooserIntent.SetFlags(ActivityFlags.GrantReadUriPermission);
            chooserIntent.SetFlags(ActivityFlags.NewTask);

            Android.App.Application.Context.StartActivity(chooserIntent);

            return Task.FromResult(true);
        }
    }
}