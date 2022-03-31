using DuoNotes.Interfaces;
using DuoNotes.iOS.Dependencies;

using Foundation;

using System.Threading.Tasks;

using UIKit;

using Xamarin.Forms;

[assembly: Dependency(typeof(Share))]
namespace DuoNotes.iOS.Dependencies {
    internal class Share : IShare {

        public async Task Show(string title, string messge, string filePath, string ext = "") {

            var viewController = GetUIViewController();
            var items = new NSObject[] { NSObject.FromObject(title), NSUrl.FromFilename(filePath) };
            var activityController = new UIActivityViewController(items, null);

            if (activityController.PopoverPresentationController != null) {
                activityController.PopoverPresentationController.SourceView = viewController.View;
            }

            await viewController.PresentViewControllerAsync(activityController, true);
        }

        private UIViewController GetUIViewController() {

            var rootCotroller = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootCotroller.PresentedViewController == null) {
                return rootCotroller;
            } else if (rootCotroller.PresentedViewController is UINavigationController navigationController) {
                return navigationController.TopViewController;
            } else if (rootCotroller.PresentedViewController is UITabBarController tabBarController) {
                return tabBarController.SelectedViewController;
            }
            return rootCotroller.PresentedViewController;
        }
    }
}