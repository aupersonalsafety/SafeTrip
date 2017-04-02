// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SafeTrip.iOS
{
    [Register ("HoldMyHandViewController")]
    partial class HoldMyHandViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HoldMyHandButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PinTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HoldMyHandButton != null) {
                HoldMyHandButton.Dispose ();
                HoldMyHandButton = null;
            }

            if (PinTextField != null) {
                PinTextField.Dispose ();
                PinTextField = null;
            }
        }
    }
}