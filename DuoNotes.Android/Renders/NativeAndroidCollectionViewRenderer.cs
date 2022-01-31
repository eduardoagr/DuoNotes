
using Android.Content;
using DuoNotes.Droid.Renders;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using DuoNotes.Renders;

[assembly: ExportRenderer(typeof(CollectionViewRender), typeof(NativeAndroidCollectionViewRenderer))]
namespace DuoNotes.Droid.Renders {
    internal class NativeAndroidCollectionViewRenderer : CollectionViewRenderer {
        public NativeAndroidCollectionViewRenderer(Context context) : base(context) {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<ItemsView> elementChangedEvent) {
            base.OnElementChanged(elementChangedEvent);

            if (elementChangedEvent.NewElement != null) {
                this.OverScrollMode = OverScrollMode.Never;
            }
        }
    }
}