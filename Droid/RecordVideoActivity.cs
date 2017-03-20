
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace SafeTrip.Droid
{
	[Activity(Label = "RecordVideoActivity")]
	public class RecordVideoActivity : Activity
	{
		MediaRecorder recorder;

		Button finishRecordingButton;
		EditText pinNumber;

		VideoView video;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);


			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.RecordVideo);

			finishRecordingButton = FindViewById<Button>(Resource.Id.finishRecordingButton);

			pinNumber = FindViewById<EditText>(Resource.Id.PinNumber);
			video = FindViewById<VideoView>(Resource.Id.videoView);

			finishRecordingButton.Click += delegate
			{
				showKeyboard(pinNumber, this);
				//showVideoFeed();
			};

			pinNumber.TextChanged += delegate {
				if (pinNumber.Text.Length >= 4)
				{
					//Check Password
					if (pinNumber.Text == "1234")
					{
						Finish();
					}
					else
					{
						Toast.MakeText(this, "Incorrect Pin", ToastLength.Short).Show();
						char[] test = new char[0];
						pinNumber.SetText(test, 0, 0);
					}
				}
			};


		}

		protected override void OnResume()
		{
			base.OnResume();

			showVideoFeed();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (recorder != null)
			{
				recorder.Release();
				recorder.Dispose();
				recorder = null;
			}
		}

		public void showVideoFeed()
		{
			string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/" + "testVideo.mp4";

			recorder = new MediaRecorder();
			recorder.SetVideoSource(VideoSource.Camera);
			recorder.SetAudioSource(AudioSource.Mic);
			recorder.SetOutputFormat(OutputFormat.Default);
			recorder.SetVideoEncoder(VideoEncoder.Default);
			recorder.SetAudioEncoder(AudioEncoder.Default);
			recorder.SetOutputFile(path);
			recorder.SetPreviewDisplay(video.Holder.Surface);
			recorder.Prepare();
			recorder.Start();
		}

		public static void showKeyboard(EditText mEtSearch, Context context)
		{
			mEtSearch.RequestFocus();
			InputMethodManager imm = (InputMethodManager)context.GetSystemService(Activity.InputMethodService);
			imm.ShowSoftInput(mEtSearch, 0);
		}

		public override void OnBackPressed()
		{
		}
	}

}
