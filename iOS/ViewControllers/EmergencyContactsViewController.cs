using System;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class EmergencyContactsViewController : UIViewController
	{
		public EmergencyContactsViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Code to start the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif


			AddEmergencyContactButton.TouchUpInside += delegate {
				var storyBoard = UIStoryboard.FromName("ModifyContact", Foundation.NSBundle.MainBundle);
				ModifyContactViewController modifyContactViewController = (ModifyContactViewController)storyBoard.InstantiateViewController("ModifyContactViewController");

				if (modifyContactViewController != null)
				{
					modifyContactViewController.emergencyContact = new EmergencyContact();
					modifyContactViewController.emergencyContactsViewController = this;
					NavigationController.PushViewController(modifyContactViewController, true);
				}
			};
		}

		public void DismissUpdateContactViewModel(ModifyContactViewController modifyContactViewController)
		{
			NavigationController.PopViewController(true);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}
	}
}
