﻿
using FFImageLoading.Forms.Platform;

using Foundation;

using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.EffectsView;
using Syncfusion.XForms.iOS.RichTextEditor;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace DuoNotes.iOS
{
    // The UIApplicationDelegate for the application. This class i  s responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            Forms.Init();
            CachedImageRenderer.Init();
            FormsMaterial.Init();
            SfEffectsViewRenderer.Init();
            SfChipGroupRenderer.Init();
            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            SfRichTextEditorRenderer.Init();
            SfChipRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
