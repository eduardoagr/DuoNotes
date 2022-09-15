using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace DuoNotes.View.PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InsertNotebookPopUpPage : PopupPage
    {
        public InsertNotebookPopUpPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}