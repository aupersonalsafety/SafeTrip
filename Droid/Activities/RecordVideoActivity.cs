using System;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Views.InputMethods;
using Android.Widget;
using Android.Text;

namespace SafeTrip.Droid
{
	[Activity(Label = "RecordVideoActivity")]
	public class RecordVideoActivity : Activity
	{
		MediaRecorder recorder;

		//VideoView video;
		//TextureView video;

		public String pin;
		String userId;
		int attempts;
		Service service;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);


			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.RecordVideo);
			if (savedInstanceState == null)
			{
				FragmentManager.BeginTransaction()
					.Replace(Resource.Id.container, Camera2VideoFragment.newInstance())
					.Commit();
			}

			//video = FindViewById<VideoView>(Resource.Id.videoView);
			//video = FindViewById<TextureView>(Resource.Id.textureView1);
			pin = Intent.GetStringExtra("pin");
			userId = Intent.GetStringExtra("userId");

			service = new Service(userId);
			attempts = 0;

			setupUI();
		}

		protected override void OnResume()
		{
			base.OnResume();

			//showVideoFeed();
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

		private void setupUI()
		{
			Button finishRecordingButton = FindViewById<Button>(Resource.Id.finishRecordingButton);
			finishRecordingButton.Click += delegate
			{
				//showVideoFeed();
				confirmPinNumber();
			};
		}

		public void showVideoFeed()
		{
			string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/" + "testVideo.mp4";

			recorder = new MediaRecorder();

			//var test = GetCameraInstance();
			//test.SetDisplayOrientation(90);

			//var test = Android.Hardware.Camera.Open();
			//Android.Hardware.Camera.Parameters parameters = test.GetParameters();
			//parameters.SetRotation(90);
			//recorder.SetCamera(test);

			//recorder.SetVideoSource(VideoSource.Camera);



			//CameraManager manager = (CameraManager)GetSystemService(CameraService);
			//String[] cameras = manager.GetCameraIdList();
			//manager.OpenCamera(cameras[0], , null);
			//manager

			//recorder.SetVideoSource(VideoSource.Camera);
			//recorder.SetAudioSource(AudioSource.Mic);
			////recorder.SetOutputFormat(OutputFormat.Default);
			////recorder.SetVideoEncoder(VideoEncoder.Default);
			////recorder.SetAudioEncoder(AudioEncoder.Default);
			//recorder.SetOutputFile(path);
			//recorder.SetPreviewDisplay(video.Holder.Surface);
			////recorder.SetOrientationHint(0);
			//CamcorderProfile profile = CamcorderProfile.Get(CamcorderQuality.High);
			//profile.Duration = 2000;
			//profile.VideoFrameHeight = video.Height;
			//profile.VideoFrameWidth = video.Width;
			//recorder.SetProfile(profile);
			//recorder.Prepare();
			//recorder.Start();
		}

		public static void showKeyboard(EditText mEtSearch, Context context)
		{
			mEtSearch.RequestFocus();
			InputMethodManager imm = (InputMethodManager)context.GetSystemService(InputMethodService);
			imm.ShowSoftInput(mEtSearch, 0);
		}

		public override void OnBackPressed()
		{
			
		}

		private void confirmPinNumber()
		{
			if (attempts > 5)
			{
				service.ContactEmergencyContacts();
				return;
			}
			AlertDialog.Builder alert = new AlertDialog.Builder(this);
			alert.SetTitle("Confirm Pin");

			var lengthFilter = new IInputFilter[] { new InputFilterLengthFilter(4) };

			LinearLayout layout = new LinearLayout(this);
			layout.Orientation = Android.Widget.Orientation.Vertical;
			EditText pinEditText = new EditText(this);
			pinEditText.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.WrapContent);
			pinEditText.Hint = "Enter Pin";
			pinEditText.SetFilters(lengthFilter);
			pinEditText.InputType = InputTypes.NumberVariationPassword;
			layout.AddView(pinEditText);

			alert.SetView(layout);

			alert.SetNegativeButton("Cancel", (sender, e) =>
			{
				
			});

			alert.SetPositiveButton("Done", (sender, e) =>
			{
				String enteredPin = pinEditText.Text;

				if (pin.Equals(enteredPin))
				{
					Finish();
				}
				else
				{
					attempts++;
					confirmPinNumber();
				}

			});
			AlertDialog dialog = alert.Create();
			dialog.Show();
		}
	}
}
