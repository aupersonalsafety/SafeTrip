﻿using System;

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



			//SubmitButton.TouchUpInside += delegate
			//{
			//	EmergencyContact emergencyContact = new EmergencyContact(ContactIDValue, FirstNameTextBox.Text, LastNameTextBox.Text, PhoneNumberTextBox.Text, EmailTextBox.Text);
			//	service.SaveOrUpdateContact(emergencyContact);
			//};

		}


		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}
	}
}
