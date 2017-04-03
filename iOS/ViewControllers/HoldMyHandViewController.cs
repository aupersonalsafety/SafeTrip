﻿using System;

using UIKit;

namespace SafeTrip.iOS
{

	public partial class HoldMyHandViewController : UIViewController
	{
		int attempts;
		bool success;
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
				PinTextField.BecomeFirstResponder();
			};
			HoldMyHandButton.TouchUpOutside += (object sender, EventArgs e) =>
			{
				PinTextField.BecomeFirstResponder();
			};

			PinTextField.EditingChanged += (object sender, EventArgs e) =>
			{
				if (PinTextField.Text.Length >= 4)
				{
					if (PinTextField.Text != "1234")
					{
						var alert = UIAlertController.Create("Incorrect PIN", "Incorrect PIN", UIAlertControllerStyle.Alert);
						alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
						PresentViewController(alert, true, null);
						attempts++;
					}
					else
					{
						success = true;
						//Exit
					}
					if (!success && attempts >= 5)
					{
						var alert = UIAlertController.Create("Contacting Emergency Contacts", "Contacting Emergency Contacts", UIAlertControllerStyle.Alert);
						alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
						PresentViewController(alert, true, null);
						PinTextField.Enabled = false;
						//ALERT EVERYONE
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

