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

			Title = "SafeTrip";

			//SubmitButton.AccessibilityIdentifier = "submitTextButton";
			//SubmitButton.TouchUpInside += delegate
			//{
			//	ResultLabel.Text = MessageTextBox.Text;
			//	service.SendSMSMessage(MessageTextBox.Text, PhoneNumberTextBox.Text);
			//};

			//GetPositionButton.TouchUpInside += delegate {
			//	setCurrentPosition();

			//	//service.monitorLocation();
			//};

			EmergencyContactsButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				var storyBoard = UIStoryboard.FromName("EmergencyContactsMenu", Foundation.NSBundle.MainBundle);
				EmergencyContactsViewController emergencyContactsViewController = (EmergencyContactsViewController) storyBoard.InstantiateViewController("EmergencyContactsViewController");
				emergencyContactsViewController.userId = 1;

				if (emergencyContactsViewController != null)
				{
					NavigationController.PushViewController(emergencyContactsViewController, true);
				}
			};

			HoldMyHandButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				var storyBoard = UIStoryboard.FromName("HoldMyHand", Foundation.NSBundle.MainBundle);
				HoldMyHandViewController holdMyHandViewController = (HoldMyHandViewController)storyBoard.InstantiateViewController("HoldMyHandViewController");

				if (holdMyHandViewController != null)
				{
					NavigationController.PushViewController(holdMyHandViewController, true);
				}
			};

			PanicButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				useCamera();
			};
		}

		//public async void setCurrentPosition()
		//{
		//	currentPosition = await service.getGlobalPosition();
		//	LatitudeLabel.Text = currentPosition.Latitude.ToString();
		//	LongitudeLabel.Text = currentPosition.Longitude.ToString();
		//}


		public async void useCamera()
		{
			await service.recordVideo();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}
	}
}
