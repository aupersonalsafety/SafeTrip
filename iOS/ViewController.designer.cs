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
        UIKit.UIButton EmergencyContactsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GetPositionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LatitudeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LongitudeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField MessageTextBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton PanicButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PhoneNumberTextBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ResultLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SubmitButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (EmergencyContactsButton != null) {
                EmergencyContactsButton.Dispose ();
                EmergencyContactsButton = null;
            }

            if (GetPositionButton != null) {
                GetPositionButton.Dispose ();
                GetPositionButton = null;
            }

            if (LatitudeLabel != null) {
                LatitudeLabel.Dispose ();
                LatitudeLabel = null;
            }

            if (LongitudeLabel != null) {
                LongitudeLabel.Dispose ();
                LongitudeLabel = null;
            }

            if (MessageTextBox != null) {
                MessageTextBox.Dispose ();
                MessageTextBox = null;
            }

            if (PanicButton != null) {
                PanicButton.Dispose ();
                PanicButton = null;
            }

            if (PhoneNumberTextBox != null) {
                PhoneNumberTextBox.Dispose ();
                PhoneNumberTextBox = null;
            }

            if (ResultLabel != null) {
                ResultLabel.Dispose ();
                ResultLabel = null;
            }

            if (SubmitButton != null) {
                SubmitButton.Dispose ();
                SubmitButton = null;
            }
        }
    }
}