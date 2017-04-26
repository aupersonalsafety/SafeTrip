using System;
using Auth0.SDK;
using System.Threading.Tasks;

using UIKit;
using Foundation;

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

			var defaults = NSUserDefaults.StandardUserDefaults;

			if (defaults.StringForKey("userId") != null && defaults.StringForKey("userToken") != null)
			{
				userId = defaults.StringForKey("userId");
				service.userId = userId;
				userToken = defaults.StringForKey("userToken");
                getPin();
			}
			else
			{
				presentLogin();
			}

            this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem("Settings", UIBarButtonItemStyle.Plain, (sender, args) =>
				{
					var storyBoard = UIStoryboard.FromName("SettingsStoryboard", Foundation.NSBundle.MainBundle);
					SettingsViewController settingsViewController = (SettingsViewController) storyBoard.InstantiateViewController("SettingsViewController");
					settingsViewController.service = service;
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
		}

		public void useCamera()
		{
			var storyBoard = UIStoryboard.FromName("Camera", Foundation.NSBundle.MainBundle);
			CameraViewController cameraViewController = (CameraViewController)storyBoard.InstantiateViewController("CameraViewController");
			cameraViewController.owner = this;
			cameraViewController.userId = userId;
			cameraViewController.pin = pin;
			cameraViewController.service = service;

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

		private async void getUserInfo(Auth0User user)
		{
			userToken = user.Auth0AccessToken;
			userId = user.Profile["user_id"].ToString();
			service.userId = userId;
			var defaults = NSUserDefaults.StandardUserDefaults;
			defaults.SetString(userId, "userId");
			defaults.SetString(userToken, "userToken");
			defaults.Synchronize(); 

			await service.createUser();
			await service.updatePin("1234");
            getPin();

			var alert = UIAlertController.Create("Pin Number Set", "Your pin number has been set to 1234 by default. You can change this in settings.", UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
			PresentViewController(alert, true, null);
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
			string fetchedPin = await service.getPin();
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

		private void displayDefaultPinMsg(String defaultMsg)
		{
			var defMsg = UIAlertController.Create("Note: ", defaultMsg, UIAlertControllerStyle.Alert);
			defMsg.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
			PresentViewController(defMsg, true, null);
		}

		public void updatePin(String pinIn)
		{
			pin = pinIn;
		}
	}
}
