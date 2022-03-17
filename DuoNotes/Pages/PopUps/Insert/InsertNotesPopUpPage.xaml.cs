
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace DuoNotes.View.PopUps {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InsertNotesPopUpPage : PopupPage {
        public InsertNotesPopUpPage() {
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