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
					displayPinAlert(true);
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
			UserTimeEstimateTextField.Text = "";
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
				displayPinAlert(false);
			}
			else
			{
				var alert = UIAlertController.Create("Invalid Time", "Please enter the number of minutes you think it will take as a whole number.", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
				PresentViewController(alert, true, null);
			}

		}
		public void displayContactingEmergencyContacts()
		{
			var alert = UIAlertController.Create("Contacting emergency contacts", "Too many attempts have been made or time has expired. Contacting Emergency Contacts.", UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
			PresentViewController(alert, true, null);
			UserTimeEstimateTextField.Text = "";
		}

		public void displayExtendTimeAlert()
		{
			string textfieldText = "";
			var alert = UIAlertController.Create(title: "Extend Time", message: "Enter the amount of time you wish to add on.", preferredStyle: UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, (field) =>
			{
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

			alert.AddTextField((field) =>
			{
				//field = AddTimeTextField;
				field.KeyboardType = UIKeyboardType.NumberPad;
				field.EditingChanged += delegate
				{
					textfieldText = field.Text;
				};
			});
			PresentViewController(alert, true, null);
		}

		public void displayPinAlert(bool unlimitedAttempts)
		{
			var alert = new UIAlertController();
			if (unlimitedAttempts)
			{
				alert = UIAlertController.Create(title: "Enter Pin", message: "Please enter your pin number.",
													 preferredStyle: UIAlertControllerStyle.Alert);
				alert.AddTextField((field) =>
					{
						field.KeyboardType = UIKeyboardType.NumberPad;
						field.SecureTextEntry = true;
						field.EditingChanged += delegate
						{
							string pinTextFieldText = field.Text;
							if (pinTextFieldText.Length >= 4)
							{
								if (pinTextFieldText == pin)
								{
									DismissViewController(true, () =>
									{
										displayExtendTimeAlert();
									});
								}
								else
								{
									attempts++;
									field.Text = "";
								}
							}

						};
					});
				alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (okayButton) =>
				{

				}));
			}
			else
			{
				alert = UIAlertController.Create(title: "Enter Pin", message: "Please enter your pin number. (You have a limited amount of attempts and time to complete this action)",
													 preferredStyle: UIAlertControllerStyle.Alert);
				alert.AddTextField((field) =>
				{
					field.KeyboardType = UIKeyboardType.NumberPad;
					field.SecureTextEntry = true;
					field.EditingChanged += delegate
					{
						string pinTextFieldText = field.Text;
						if (pinTextFieldText.Length >= 4)
						{
							if (pinTextFieldText == pin)
							{
								DismissViewController(true, null);
								attempts = 0;
								TimerSetLabel.Text = "";
								StartSafeTripButton.SetTitle("Start SafeTrip Timer", UIControlState.Normal);
								timerSet = false;
							}
							else
							{
								if (attempts >= 5)
								{
									DismissViewController(true, () =>
										{
											attempts = 0;
											TimerSetLabel.Text = "";
											StartSafeTripButton.SetTitle("Start SafeTrip Timer", UIControlState.Normal);
											timerSet = false;
											displayContactingEmergencyContacts();
										});
								}
								else
								{
									attempts++;
									field.Text = "";
								}

							}
						}

					};
				});
			}
            PresentViewController(alert, true, null);
		}


		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

