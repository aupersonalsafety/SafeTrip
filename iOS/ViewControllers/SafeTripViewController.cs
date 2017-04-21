using System;

using UIKit;

namespace SafeTrip.iOS
{

	public partial class SafeTripViewController : UIViewController
	{
		int attempts;
		bool success;
		public string pin;
		bool timerSet = false;

		public SafeTrip.Service service;

		public SafeTripViewController() : base("SafeTripViewController", null)
		{
		}

		public SafeTripViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			attempts = 0;
			success = false;

			CalculateTravelTimeButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				getTravelTime();
			};

		}

		public async void getTravelTime()
		{
			service.getTravelTime("357 E Thach Ave Auburn Al");
			EstimatedTravelTimeLabel.Text = "Estimated Travel Time: " + "12" + "minutes";
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

