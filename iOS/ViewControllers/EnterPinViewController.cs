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
			// Perform any additional setup after loading the view, typically from a nib.
			attempts = 0;
			success = false;


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
