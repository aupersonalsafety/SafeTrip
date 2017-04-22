
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SafeTrip.Droid
{
	[Activity(Label = "PinActivity")]
	public class PinActivity : Activity
	{
		Button verifyPinButton;
		EditText pintEditText;
		int pin;
		int maxAttempts = 5;
		int attempts;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.PinLayout);

			//FIXME
			//fetch pin from database?
			pin = 1234;

			attempts = 0;

			pintEditText = FindViewById<EditText>(Resource.Id.pinEditText);

			verifyPinButton = FindViewById<Button>(Resource.Id.verifyPinButton);
			verifyPinButton.Click += delegate {
				verifyPin();
			};
		}

		private void verifyPin()
		{
			if (attempts != maxAttempts)
			{
				if (Int32.Parse(pintEditText.Text) == pin)
				{
					pintEditText.Text = "";
					Finish();
				}
				else
				{
					attempts++;
					pintEditText.Text = "";
					displayToast("Invalid pin. Please try again");
				}
			}
			else
			{
				displayToast("You have entered too many invalid pins. Alerting Emergency Contacts");
				verifyPinButton.Enabled = false;
			}
		}

		private void displayToast(String message)
		{
			Toast.MakeText(this, message, ToastLength.Short).Show();
		}
	}
}