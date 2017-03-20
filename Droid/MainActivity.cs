using Android.App;
using Android.Widget;
using Android.OS;

namespace SafeTrip.Droid
{
	[Activity(Label = "SafeTrip", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);
            Button button2 = FindViewById<Button>(Resource.Id.myButton2);
			Button recordButton = FindViewById<Button>(Resource.Id.recordVideoButton);

            EditText nameEditText = FindViewById<EditText>(Resource.Id.editText1);
            EditText messageEditText = FindViewById<EditText>(Resource.Id.editText2);
            EditText recipientPhoneNumberEditText = FindViewById<EditText>(Resource.Id.editText3);
            Service service = new Service();

            button.Click += async delegate {                
                string greeting = await service.SayHello(nameEditText.Text);
                Toast.MakeText(this, greeting, ToastLength.Long).Show();
            };

            button2.Click += delegate
            {
				service.SendSMSMessage(messageEditText.Text, recipientPhoneNumberEditText.Text);
            };

			recordButton.Click += delegate {
				//service.recordVideo();
				StartActivity(typeof(RecordVideoActivity));
			};
		}
	}
}

