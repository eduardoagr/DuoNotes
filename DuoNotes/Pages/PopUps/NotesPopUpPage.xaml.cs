
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace DuoNotes.View.PopUps {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPopUp : PopupPage {
        public NotesPopUp() {
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