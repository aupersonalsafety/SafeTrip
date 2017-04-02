//This file was taken from Camera2VideoSample found at https://developer.xamarin.com/samples/monodroid/android5.0/Camera2Basic/
using System;
using Android.Hardware.Camera2;
using Android.Widget;

namespace SafeTrip.Droid
{
	public class MyCameraStateCallback : CameraDevice.StateCallback
	{
		Camera2VideoFragment fragment;
		public MyCameraStateCallback(Camera2VideoFragment frag)
		{
			fragment = frag;
		}
		public override void OnOpened (CameraDevice camera)
		{
			fragment.cameraDevice = camera;
			fragment.startPreview ();
			fragment.cameraOpenCloseLock.Release ();
			if (null != fragment.textureView) 
				fragment.configureTransform (fragment.textureView.Width, fragment.textureView.Height);
		}

		public override void OnDisconnected (CameraDevice camera)
		{
			fragment.cameraOpenCloseLock.Release ();
			camera.Close ();
			fragment.cameraDevice = null;
		}

		public override void OnError (CameraDevice camera, CameraError error)
		{
			fragment.cameraOpenCloseLock.Release ();
			camera.Close ();
			fragment.cameraDevice = null;
			if (null != fragment.Activity) 
				fragment.Activity.Finish ();
		}


	}
}

