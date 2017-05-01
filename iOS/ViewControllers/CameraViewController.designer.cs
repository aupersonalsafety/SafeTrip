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
    [Register ("CameraViewController")]
    partial class CameraViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FinishRecordingButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FinishRecordingButton != null) {
                FinishRecordingButton.Dispose ();
                FinishRecordingButton = null;
            }

            if (PreviewView != null) {
                PreviewView.Dispose ();
                PreviewView = null;
            }
        }
    }
}