//This file was taken from Camera2VideoSample found at https://developer.xamarin.com/samples/monodroid/android5.0/Camera2Basic/
using System;
using Android.Hardware.Camera2;
using Android.Widget;

namespace SafeTrip.Droid
{
	public class PreviewCaptureStateCallback: CameraCaptureSession.StateCallback
	{
		Camera2VideoFragment fragment;
		public PreviewCaptureStateCallback(Camera2VideoFragment frag)
		{
			fragment = frag;
		}
		public override void OnConfigured (CameraCaptureSession session)
		{
			fragment.previewSession = session;
			fragment.updatePreview ();

		}

		public override void OnConfigureFailed (CameraCaptureSession session)
		{
			if (null != fragment.Activity) 
				Toast.MakeText (fragment.Activity, "Failed", ToastLength.Short).Show ();
		}
	}
}

