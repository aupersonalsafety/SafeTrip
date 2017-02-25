using System;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class ModifyContactViewController : UIViewController
	{
		public EmergencyContactsViewController emergencyContactsViewController;

		public EmergencyContact emergencyContact;
		SafeTrip.Service service = new SafeTrip.Service();

		public ModifyContactViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{


			UpdateContactButton.TouchUpInside += delegate
			{
				emergencyContact = new EmergencyContact(emergencyContact.ContactID, FirstNameTextField.Text, LastNameTextField.Text, PhoneNumberTextField.Text, EmailTextField.Text);
				//service.SaveOrUpdateContact(emergencyContact);
				UpdateContact(emergencyContact);

			};


			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public async void UpdateContact(EmergencyContact emergencyContactIn)
		{
			if (await service.SaveOrUpdateContact(emergencyContactIn) == 1)
			{
				emergencyContactsViewController.DismissUpdateContactViewModel(this);
			}
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

