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
        void ReleaseDesignerOutlets ()
        {
            if (PreviewView != null) {
                PreviewView.Dispose ();
                PreviewView = null;
            }

            if (RecordButton != null) {
                RecordButton.Dispose ();
                RecordButton = null;
            }
        }
    }
}