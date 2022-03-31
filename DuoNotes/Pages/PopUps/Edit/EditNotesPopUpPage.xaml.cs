using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace DuoNotes.Pages.PopUps.Edit {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditNotesPopUpPage : PopupPage {
        public EditNotesPopUpPage() {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed() {
            return true;
        }

        protected override bool OnBackgroundClicked() {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return false;
        }
    }
}