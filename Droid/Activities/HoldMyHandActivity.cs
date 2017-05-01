
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace SafeTrip.Droid
{
	[Activity(Label = "HoldMyHandActivity")]
	public class HoldMyHandActivity : Activity
	{
		int attempts;
		string pin;
		Service service;
		string userId;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			userId = Intent.GetStringExtra("userId");
			pin = Intent.GetStringExtra("pin");

			service = new Service(userId);
			attempts = 0;

			SetContentView(Resource.Layout.HoldMyHand);
			Button holdMyHandButton = FindViewById<Button>(Resource.Id.holdMyHandButton);
			holdMyHandButton.Touch+= (s, e) =>
			{
			    var handled = false;
			    if (e.Event.Action == MotionEventActions.Down)
				{
					// do stuff
					handled = true;
				}
				else if (e.Event.Action == MotionEventActions.Up)
				{
					// do other stuff
					//Toast.MakeText(this, "test", ToastLength.Long).Show();
					confirmPinNumber();
					handled = true;
				}

				e.Handled = handled;
			};


			// Create your application here
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
