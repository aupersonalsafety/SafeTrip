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
    [Register ("EmergencyContactsViewController")]
    partial class EmergencyContactsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddNewContactButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddNewContactButton != null) {
                AddNewContactButton.Dispose ();
                AddNewContactButton = null;
            }
        }
    }
}