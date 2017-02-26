using System;

using Plugin.Contacts.Abstractions;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class ModifyContactViewController : UIViewController
	{
		public EmergencyContactsViewController emergencyContactsViewController;

		public EmergencyContact emergencyContact;
		SafeTrip.Service service = new SafeTrip.Service();
		int? emergencyContactID;

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

			AddressBookButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				var storyBoard = UIStoryboard.FromName("ContactsSelector", null);
				ContactsTableViewController contactsTableViewController = (ContactsTableViewController)storyBoard.InstantiateViewController("ContactsTableViewController");
				contactsTableViewController.modifyContactsViewController = this;

				if (contactsTableViewController != null)
				{
					NavigationController.PushViewController(contactsTableViewController, true);
				}
			};

			LoadEmergencyContact(emergencyContact);
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public async void UpdateContact(EmergencyContact emergencyContactIn)
		{
			if (await service.SaveOrUpdateContact(emergencyContactIn) == 1)
			{
				emergencyContactsViewController.DismissUpdateContactViewModel();
			}
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public void DismissUpdateContactViewModel(Contact contact)
		{
			NavigationController.PopViewController(true);

			FirstNameTextField.Text = contact.FirstName;
			LastNameTextField.Text = contact.LastName;
			if (contact.Phones.Count > 0)
			{
				PhoneNumberTextField.Text = contact.Phones[0].Number;
			}
			if (contact.Emails.Count > 0)
			{
				EmailTextField.Text = contact.Emails[0].Address;
			}
		}

		public void LoadEmergencyContact(EmergencyContact emergencyContact)
		{
			emergencyContactID = emergencyContact.ContactID;
			FirstNameTextField.Text = emergencyContact.FirstName;
			LastNameTextField.Text = emergencyContact.LastName;
			PhoneNumberTextField.Text = emergencyContact.PhoneNumber;
			EmailTextField.Text = emergencyContact.Email;
		}
	}
}

