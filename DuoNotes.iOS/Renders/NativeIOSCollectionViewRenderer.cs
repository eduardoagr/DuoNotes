using DuoNotes.iOS.Renders;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CollectionView), typeof(NativeIOSCollectionViewRenderer))]
namespace DuoNotes.iOS.Renders {
    internal class NativeIOSCollectionViewRenderer : CollectionViewRenderer {

        public NativeIOSCollectionViewRenderer() {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<GroupableItemsView> e) {
            base.OnElementChanged(e);

            if (e.NewElement != null) {
                Controller.CollectionView.Bounces = false;
            }
        }
    }


}