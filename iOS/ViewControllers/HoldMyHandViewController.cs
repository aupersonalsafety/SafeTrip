using System;

using UIKit;

namespace SafeTrip.iOS
{

	public partial class HoldMyHandViewController : UIViewController
	{
		int attempts;
		bool success;
		public string pin;
		bool timerSet = false;

		public SafeTrip.Service service;

		public HoldMyHandViewController() : base("HoldMyHandViewController", null)
		{
		}

		public HoldMyHandViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			attempts = 0;
			success = false;

			HoldMyHandButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				if (timerSet == false)
				{

                    displayPinAlert();
					//StartTimer
					timerSet = true;
				}
			};
			HoldMyHandButton.TouchUpOutside += (object sender, EventArgs e) =>
			{
				if (timerSet == false)
				{
					displayPinAlert();
					//StartTimer
					timerSet = true;
				}
			};

		}

		public void displayPinAlert()
		{
			var alert = UIAlertController.Create(title: "Enter Pin", message: "Please enter your pin number. (You have a limited amount of attempts and time to complete this action)",
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
							timerSet = false;
							//Cancel Timer
						}
						else
						{
							if (attempts >= 5)
							{
								DismissViewController(true, () =>
									{
										attempts = 0;
										timerSet = false;
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

			PresentViewController(alert, true, null);
		}


		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

