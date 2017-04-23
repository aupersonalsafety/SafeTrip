using System;

using UIKit;

namespace SafeTrip.iOS
{

	public partial class SafeTripViewController : UIViewController
	{
		int attempts;
		bool success;
		public string pin;
		bool timerSet = false;
		DateTime estimatedArrivalTime;

		public SafeTrip.Service service;

		public SafeTripViewController() : base("SafeTripViewController", null)
		{
		}

		public SafeTripViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			attempts = 0;
			success = false;

			CalculateTravelTimeButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				getTravelTime();
			};

			StartSafeTripButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				if (!timerSet)
				{
					StartSafeTrip();
					timerSet = true;
				}
				else
				{
					string textfieldText = "";
					UITextField AddTimeTextField = new UITextField();
					var alert = UIAlertController.Create(title: "Extend Time", message: "Enter the amount of time you wish to add on.", preferredStyle: UIAlertControllerStyle.Alert);
					alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, (field) => {
						int addedMinutes;
						bool isNumber = Int32.TryParse(textfieldText, out addedMinutes);
						if (isNumber)
						{
							estimatedArrivalTime = estimatedArrivalTime.AddMinutes(addedMinutes);
							TimerSetLabel.Text = "Expected Arival Time: " + estimatedArrivalTime.ToShortTimeString() + " " + estimatedArrivalTime.ToShortDateString();
							//Extend Server Timer
						}
						else
						{
							
						}
					}));

					alert.AddTextField((field) => {
						//field = AddTimeTextField;
						field.KeyboardType = UIKeyboardType.NumberPad;
						field.EditingChanged += delegate {
							textfieldText = field.Text;
						};
					});
					PresentViewController(alert, true, null);

				}
			};

		}

		public async void getTravelTime()
		{
			
			int estimatedTime = await service.getTravelTime(DesinationTextField.Text);
			EstimatedTravelTimeLabel.Text = "Estimated Travel Time: " + estimatedTime + " minutes";
			UserTimeEstimateTextField.Text = estimatedTime.ToString();
		}

		public async void StartSafeTrip()
		{
			int time;
			bool successfulParse = Int32.TryParse(UserTimeEstimateTextField.Text, out time);
			if (successfulParse)
			{
				DateTime now = DateTime.Now;
				estimatedArrivalTime = now.AddMinutes(time);
				TimerSetLabel.Text = "Expected Arival Time: " + estimatedArrivalTime.ToShortTimeString() + " " + estimatedArrivalTime.ToShortDateString();
				StartSafeTripButton.SetTitle("Extend Time", UIControlState.Normal);

				while (now.ToShortTimeString() != estimatedArrivalTime.ToShortTimeString())
				{
					now = DateTime.Now;
					TimeSpan timespan = estimatedArrivalTime.Subtract(now);
					await service.setTimer(timespan.Seconds);
				}

				//Show pin screen

				var alert = UIAlertController.Create("Time expired", "Time has expired.", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
				PresentViewController(alert, true, null);
			}
			else
			{
				var alert = UIAlertController.Create("Invalid Time", "Please enter the number of minutes you think it will take as a whole number.", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
				PresentViewController(alert, true, null);
				UserTimeEstimateTextField.Text = "";
			}
			
		}


		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

