
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
	[Activity(Label = "SafeTripActivity")]
	public class SafeTripActivity : Activity
	{
		int attempts;
		bool success;
		string pin;
		bool timerSet = false;
		DateTime estimatedArrivalTime;

		string userId;
		Service service;


		EditText destination;
		Button estimateTravelTimeButton;
		TextView estimatedTravelTime;
		EditText userEstimatedTime;
		Button startSafeTripButton;
		TextView estimatedArrivalTimeTextField;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			userId = Intent.GetStringExtra("userId");
			pin = Intent.GetStringExtra("pin");

			service = new Service(userId);
			attempts = 0;

			SetContentView(Resource.Layout.SafeTrip);

			destination = FindViewById<EditText>(Resource.Id.DestinationEditText);

			estimateTravelTimeButton = FindViewById<Button>(Resource.Id.estimateTravelTimeButton);
			estimatedTravelTime = FindViewById<TextView>(Resource.Id.estimatedTravelTimeTextView);
			userEstimatedTime = FindViewById<EditText>(Resource.Id.UserEstimatedTime);
			startSafeTripButton = FindViewById<Button>(Resource.Id.safeTripTimerButton);
			estimatedArrivalTimeTextField = FindViewById<TextView>(Resource.Id.estimatedArrivalTime);

			estimateTravelTimeButton.Click += delegate
			{
				setEstimatedTravelTime();
			};

			startSafeTripButton.Click += delegate
			{
				if (!timerSet)
				{
                    StartSafeTrip();
					timerSet = true;
				}
				else
				{
					confirmPinNumber();
				}
				//StartSafeTrip();
			};
		}

		public async void setEstimatedTravelTime()
		{
			int estimatedTime = (int)Math.Round((Convert.ToDouble(await service.getTravelTime(destination.Text)) / 60), 0);
			estimatedTravelTime.Text = "Estimated Travel Time: " + estimatedTime + " minutes.";
			userEstimatedTime.Text = estimatedTime.ToString();
		}

		public async void StartSafeTrip()
		{

			int time;
			bool successfulParse = Int32.TryParse(userEstimatedTime.Text, out time);
			userEstimatedTime.Text = "";
			if (successfulParse)
			{
				DateTime now = DateTime.Now;
				estimatedArrivalTime = now.AddMinutes(time);
				estimatedArrivalTimeTextField.Text = "Expected Arival Time: " + estimatedArrivalTime.ToShortTimeString() + " " + estimatedArrivalTime.ToShortDateString();
				startSafeTripButton.Text = "Extend Timer";

				while (now.ToShortTimeString() != estimatedArrivalTime.ToShortTimeString())
				{
					now = DateTime.Now;
					TimeSpan timespan = estimatedArrivalTime.Subtract(now);
					await service.setTimer(timespan.Seconds);
				}

				Toast.MakeText(this, "Timer Expired. Alerting contacts.", ToastLength.Long).Show();
				//Need to change this to an alert so it stays on the screen. Look at confirm pin for example.
			}
			else
			{
				Toast.MakeText(this, "Not a valid amount of time", ToastLength.Long).Show();

			}
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
					extendTime();
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
		private void extendTime()
		{
			AlertDialog.Builder alert = new AlertDialog.Builder(this);
			alert.SetTitle("Enter amount of time");

			LinearLayout layout = new LinearLayout(this);
			layout.Orientation = Android.Widget.Orientation.Vertical;
			EditText editTextMinutes = new EditText(this);
			editTextMinutes.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.WrapContent);
			editTextMinutes.Hint = "Enter time in minutes";
			editTextMinutes.InputType = InputTypes.ClassNumber;
			layout.AddView(editTextMinutes);

			alert.SetView(layout);

			alert.SetNegativeButton("Cancel", (sender, e) =>
			{

			});

			alert.SetPositiveButton("Done", (sender, e) =>
			{
				int addedMinutes;
				bool isNumber = Int32.TryParse(editTextMinutes.Text, out addedMinutes);
				if (isNumber)
				{
					estimatedArrivalTime = estimatedArrivalTime.AddMinutes(addedMinutes);
					estimatedArrivalTimeTextField.Text = "Expected Arival Time: " + estimatedArrivalTime.ToShortTimeString() + " " + estimatedArrivalTime.ToShortDateString();
					//Extend servertimer
				}
				else
				{
				}

			});
			AlertDialog dialog = alert.Create();
			dialog.Show();
			
		}
	}
}