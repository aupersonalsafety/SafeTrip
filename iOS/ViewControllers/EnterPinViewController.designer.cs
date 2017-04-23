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
    [Register ("EnterPinViewController")]
    partial class EnterPinViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PinTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (PinTextField != null) {
                PinTextField.Dispose ();
                PinTextField = null;
            }
        }
    }
}