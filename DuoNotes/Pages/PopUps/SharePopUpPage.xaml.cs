using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace DuoNotes.Pages.PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SharePopUpPage : PopupPage
    {
        public SharePopUpPage()
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