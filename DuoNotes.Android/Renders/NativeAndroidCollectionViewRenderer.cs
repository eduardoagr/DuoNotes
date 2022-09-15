
using Android.Content;
using Android.Views;

using DuoNotes.Droid.Renders;
using DuoNotes.Renders;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CollectionViewRender), typeof(NativeAndroidCollectionViewRenderer))]
namespace DuoNotes.Droid.Renders
{
    internal class NativeAndroidCollectionViewRenderer : CollectionViewRenderer
    {
        public NativeAndroidCollectionViewRenderer(Context context) : base(context)
        {
        }
        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return false;
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            return false;
        }
    }
}