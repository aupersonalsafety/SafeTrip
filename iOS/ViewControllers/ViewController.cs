using System;
using Auth0.SDK;
using System.Threading.Tasks;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class ViewController : UIViewController
	{
		//SafeTrip.Service service = new SafeTrip.Service();
		//GlobalPosition currentPosition;
		Service service = new Service();
		String userToken;
		String userId;

		private string pin = "-1"; 

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		private Auth0.SDK.Auth0Client client = new Auth0.SDK.Auth0Client("aupersonalsafety.auth0.com",
																		 "n4kXJEiHpBL3v1e0p0cM6pj8icidoZzo");

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = "SafeTrip";

			getPin();

			userToken = "";
			userId = "";

            this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem("Settings", UIBarButtonItemStyle.Plain, (sender, args) =>
				{
					var storyBoard = UIStoryboard.FromName("SettingsStoryboard", Foundation.NSBundle.MainBundle);
					SettingsViewController settingsViewController = (SettingsViewController) storyBoard.InstantiateViewController("SettingsViewController");
					if (settingsViewController != null)
					{
						settingsViewController.presentingViewController = this;
						settingsViewController.client = client;
						settingsViewController.pin = pin;
						settingsViewController.userId = userId;
						NavigationController.PushViewController(settingsViewController, true);
					}
				})
			, true);

			HoldMyHandButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				var storyBoard = UIStoryboard.FromName("HoldMyHand", Foundation.NSBundle.MainBundle);
				HoldMyHandViewController holdMyHandViewController = (HoldMyHandViewController)storyBoard.InstantiateViewController("HoldMyHandViewController");
				holdMyHandViewController.service = service;
				holdMyHandViewController.pin = pin;
				holdMyHandViewController.userId = userId;
				if (holdMyHandViewController != null)
				{
					NavigationController.PushViewController(holdMyHandViewController, true);
				}
			};

			SafeTripButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				var storyBoard = UIStoryboard.FromName("SafeTrip", Foundation.NSBundle.MainBundle);
				SafeTripViewController safeTripViewController = (SafeTripViewController)storyBoard.InstantiateViewController("SafeTripViewController");
				safeTripViewController.service = service;
				safeTripViewController.pin = pin;
				safeTripViewController.userId = userId;
				if (safeTripViewController != null)
				{
					NavigationController.PushViewController(safeTripViewController, true);
				}
			};

			PanicButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				useCamera();
			};

			PanicButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				useCamera();
			};
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			var tempUser = client.CurrentUser;
			
			if (tempUser == null)
			{
				presentLogin();
			}
			else
			{
				getUserInfo(null);
			}
		}

		public void useCamera()
		{
			var storyBoard = UIStoryboard.FromName("Camera", Foundation.NSBundle.MainBundle);
			CameraViewController cameraViewController = (CameraViewController)storyBoard.InstantiateViewController("CameraViewController");
			cameraViewController.owner = this;
			cameraViewController.userId = userId;
			cameraViewController.pin = pin;

			if (cameraViewController != null)
			{
				NavigationController.PushViewController(cameraViewController, true);
			}
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}

		private async Task presentLogin()
		{
			try
			{
				var user = await this.client.LoginAsync(this);
				getUserInfo(user);
			}
			catch (OperationCanceledException e)
			{
				displayError(e.Message);
			}
		}

		private async void getUserInfo(Auth0User userIn)
		{
			Auth0User user = userIn;
			if (user == null)
			{
				user = client.CurrentUser;
			}
			userToken = user.Auth0AccessToken;
			userId = user.Profile["user_id"].ToString();
			await service.createUser(userId);
		}

		public void dismissCamera()
		{
			NavigationController.PopViewController(true);
		}

		public void dismissSettings()
		{
			NavigationController.PopViewController(true);
			presentLogin();
		}

		public async Task getPin()
		{
			string fetchedPin = await service.getPin(userId);
			if (fetchedPin != "-1")
			{
				pin = fetchedPin;
			}
			else
			{
				displayError("Could not fetch pin. Please close app and try again.");
			}
		}

		private void displayError(String error)
		{
			var alert = UIAlertController.Create("Error", error, UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
			PresentViewController(alert, true, null);
		}

		public void updatePin(String pinIn)
		{
			pin = pinIn;
		}
	}
}
