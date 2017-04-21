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
    [Register ("SafeTripViewController")]
    partial class SafeTripViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CalculateTravelTimeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField DesinationTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DestinationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EstimatedTravelTimeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PinTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton StartSafeTripButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField UserTimeEstimateTextField { get; set; }

        [Action ("StartSafeTripButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void StartSafeTripButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CalculateTravelTimeButton != null) {
                CalculateTravelTimeButton.Dispose ();
                CalculateTravelTimeButton = null;
            }

            if (DesinationTextField != null) {
                DesinationTextField.Dispose ();
                DesinationTextField = null;
            }

            if (DestinationLabel != null) {
                DestinationLabel.Dispose ();
                DestinationLabel = null;
            }

            if (EstimatedTravelTimeLabel != null) {
                EstimatedTravelTimeLabel.Dispose ();
                EstimatedTravelTimeLabel = null;
            }

            if (PinTextField != null) {
                PinTextField.Dispose ();
                PinTextField = null;
            }

            if (StartSafeTripButton != null) {
                StartSafeTripButton.Dispose ();
                StartSafeTripButton = null;
            }

            if (UserTimeEstimateTextField != null) {
                UserTimeEstimateTextField.Dispose ();
                UserTimeEstimateTextField = null;
            }
        }
    }
}