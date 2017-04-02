using System;

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

			PinTextField.ValueChanged += (object sender, EventArgs e) =>
			{
				if (PinTextField.Text.Length >= 4)
				{
					
					while (attempts < 5 || !success)
					{
						if (PinTextField.Text != "1234")
						{
							var test = UIAlertController.Create("Incorrect PIN", "Incorrect PIN", UIAlertControllerStyle.Alert);
							test.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
							PresentViewController(test, true, null);
							attempts++;
						}
						else
						{
							success = true;
							//Exit
						}
					}
					if (!success)
					{
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

