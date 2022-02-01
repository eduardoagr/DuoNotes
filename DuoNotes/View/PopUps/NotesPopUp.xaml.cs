using DuoNotes.Model;
using DuoNotes.ViewModel.PopUps;

using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace DuoNotes.View.PopUps {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPopUp : PopupPage {
        public NotesPopUp() {
            InitializeComponent();
        }

        public NotesPopUp(string Id) {
            InitializeComponent();

            var viewMode = new NewNotePopUpViewModel {
                RecivedSelectedNotebookID = Id
            };
            BindingContext = viewMode;
        }

        protected override bool OnBackButtonPressed() {
            return true;
        }

        protected override bool OnBackgroundClicked() {
            return false;
        }
    }
}