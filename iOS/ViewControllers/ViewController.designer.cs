// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SafeTrip.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIButton Button { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HoldMyHandButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton PanicButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SafeTripButton { get; set; }

        [Action ("HoldMyHandButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void HoldMyHandButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (HoldMyHandButton != null) {
                HoldMyHandButton.Dispose ();
                HoldMyHandButton = null;
            }

            if (PanicButton != null) {
                PanicButton.Dispose ();
                PanicButton = null;
            }

            if (SafeTripButton != null) {
                SafeTripButton.Dispose ();
                SafeTripButton = null;
            }
        }
    }
}