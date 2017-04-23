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

					var storyBoard = UIStoryboard.FromName("EnterPin", Foundation.NSBundle.MainBundle);
					EnterPinViewController enterPinViewController = (EnterPinViewController)storyBoard.InstantiateViewController("EnterPinViewController");
					enterPinViewController.service = service;
					enterPinViewController.pin = pin;
					if (enterPinViewController != null)
					{
						NavigationController.PushViewController(enterPinViewController, true);
					}

					//PinTextField.BecomeFirstResponder();
					//Start Timer
					//timerSet = true;
				}
			};
			HoldMyHandButton.TouchUpOutside += (object sender, EventArgs e) =>
			{
				if (timerSet == false)
				{
					var storyBoard = UIStoryboard.FromName("EnterPin", Foundation.NSBundle.MainBundle);
					EnterPinViewController enterPinViewController = (EnterPinViewController)storyBoard.InstantiateViewController("EnterPinViewController");
					enterPinViewController.service = service;
					enterPinViewController.pin = pin;
					if (enterPinViewController != null)
					{
						NavigationController.PushViewController(enterPinViewController, true);
					}
					//PinTextField.BecomeFirstResponder();
					//Start Timer
					//timerSet = true;
				}
			};

			PinTextField.EditingChanged += (object sender, EventArgs e) =>
			{
				if (PinTextField.Text.Length >= 4)
				{
					if (PinTextField.Text != pin)
					{
						if (attempts >= 5)
						{
							var alert = UIAlertController.Create("Too many attempts", "Contacting Emergency Contacts", UIAlertControllerStyle.Alert);

							//
							//Contact Emergency Contacts
							//service.ContactEmergencyContacts();
							//

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
						success = true;
						var alert = UIAlertController.Create("Success", "Success", UIAlertControllerStyle.Alert);
						alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
						PresentViewController(alert, true, null);
						PinTextField.Text = "";
						attempts = 0;
						timerSet = false;
						
						PinTextField.ResignFirstResponder();
					}
				}
			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

