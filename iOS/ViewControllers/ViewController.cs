using System;
using Auth0.SDK;

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

		private Auth0.SDK.Auth0Client client = new Auth0.SDK.Auth0Client("aupersonalsafety.auth0.com",
																		 "n4kXJEiHpBL3v1e0p0cM6pj8icidoZzo");

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

			LoginButton.TouchUpInside += delegate
			{

				loginWithWidgetButtonClick();
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

		private async void loginWithWidgetButtonClick()
		{
			try
			{
				var user = await this.client.LoginAsync(this);
				user.Profile["email"].ToString();
			}

			catch (OperationCanceledException e)
			{
				var okCancelAlertController = UIAlertController.Create("Must login before using SafeTrip", "", UIAlertControllerStyle.Alert);
				okCancelAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, action => loginWithWidgetButtonClick()));
				PresentViewController(okCancelAlertController, true, null);

				Console.WriteLine("Cancel ex {0}", e.Message);
			}
		}
	}
}
