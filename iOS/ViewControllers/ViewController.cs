using System;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class ViewController : UIViewController
	{
		SafeTrip.Service service = new SafeTrip.Service();
		GlobalPosition currentPosition;

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Code to start the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
#endif

			// Perform any additional setup after loading the view, typically from a nib.
			//Button.AccessibilityIdentifier = "myButton";
			//Button.TouchUpInside += delegate
			//{
			//	var title = string.Format("{0} clicks!", count++);
			//	Button.SetTitle(title, UIControlState.Normal);
			//};


			SubmitButton.AccessibilityIdentifier = "submitTextButton";
			SubmitButton.TouchUpInside += delegate
			{
				ResultLabel.Text = MessageTextBox.Text;
				service.SendSMSMessage(MessageTextBox.Text, PhoneNumberTextBox.Text);
			};

			GetPositionButton.TouchUpInside += delegate {
				setCurrentPosition();

			};


			EmergencyContactsButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				var storyBoard = UIStoryboard.FromName("EmergencyContactsMenu", Foundation.NSBundle.MainBundle);
				EmergencyContactsViewController emergencyContactsViewController = (EmergencyContactsViewController) storyBoard.InstantiateViewController("EmergencyContactsViewController");

				if (emergencyContactsViewController != null)
				{
					//emergencyContactsViewController.PhoneNumbers = PhoneNumbers;
					NavigationController.PushViewController(emergencyContactsViewController, true);
				}
			};

		}

		public async void setCurrentPosition()
		{
			currentPosition = await service.getGlobalPosition();
			LatitudeLabel.Text = currentPosition.Latitude.ToString();
			LongitudeLabel.Text = currentPosition.Longitude.ToString();
		}


		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}
	}
}
