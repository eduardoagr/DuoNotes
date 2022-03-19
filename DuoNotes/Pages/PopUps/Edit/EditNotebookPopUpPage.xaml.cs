using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace DuoNotes.Pages.PopUps.Edit {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditNotebookPopUpPage : PopupPage {
        public EditNotebookPopUpPage() {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed() {
            return true;
        }

        protected override bool OnBackgroundClicked() {
            return false;
        }
    }
}