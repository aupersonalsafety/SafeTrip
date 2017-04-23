using System;

using UIKit;

namespace SafeTrip.iOS
{

	public partial class EnterPinViewController : UIViewController
	{
		int attempts;
		bool success;
		public string pin;
		bool timerSet = false;
		public int timerValue;

		public SafeTrip.Service service;

		public EnterPinViewController() : base("EnterPinViewController", null)
		{
		}

		public EnterPinViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NavigationItem.HidesBackButton = true;
			startTimers();


			// Perform any additional setup after loading the view, typically from a nib.
			attempts = 0;
			success = false;



			PinTextField.EditingChanged += (object sender, EventArgs e) =>
			{
				if (PinTextField.Text.Length >= 4)
				{
					if (PinTextField.Text != pin)
					{
						if (attempts >= 5)
						{
							var alert = UIAlertController.Create("Too many attempts", "Contacting Emergency Contacts", UIAlertControllerStyle.Alert);


							alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
							PresentViewController(alert, true, null);
							PinTextField.Text = "";
							PinTextField.Enabled = false;
						}
						else
						{
							var alert = UIAlertController.Create("Incorrect PIN", "You have entered an incorrect PIN", UIAlertControllerStyle.Alert);
							alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
							PresentViewController(alert, true, null);
							attempts++;
							PinTextField.Text = "";
						}


					}
					else
					{
						//success = true;
						//var alert = UIAlertController.Create("Success", "Success", UIAlertControllerStyle.Alert);
						//alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
						//PresentViewController(alert, true, null);
						//PinTextField.Text = "";
						//attempts = 0;
						//timerSet = false;
						
						NavigationController.PopViewController(true);
					}
				}
			};

		}

		public async void startTimers()
		{
			int timerFinished = await service.setTimer(15);
			if (timerFinished == 1)
			{
				var alert = UIAlertController.Create("TimeExpired", "Contacting Emergency Contacts", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
				PresentViewController(alert, true, null);
			}
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
