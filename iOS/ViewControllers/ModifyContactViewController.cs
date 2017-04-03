using System;
using System.Collections.Generic;
using Plugin.Contacts.Abstractions;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class ModifyContactViewController : UIViewController
	{
		string value = "test";
		public EmergencyContactsViewController emergencyContactsViewController;

		public EmergencyContact emergencyContact;
		SafeTrip.Service service = new SafeTrip.Service();
		int? emergencyContactID;
		Dictionary<string, string> carrierDict;
		List<string> carriersList;

		public ModifyContactViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			carrierDict = new Dictionary<String, String>();
			carrierDict.Add("AT&T", "@txt.att.net");
			carrierDict.Add("T-Mobile", "@tmomail.net");
			carrierDict.Add("Virgin Mobile", "@vmobl.com");
			carrierDict.Add("Cingular", "@cingularme.com");
			carrierDict.Add("Sprint", "@messaging.sprintpcs.com");
			carrierDict.Add("Verizon", "@vtext.com");
			carrierDict.Add("Nextel", "@messaging.nextel.com");

			carriersList = new List<string>();
			carriersList.AddRange(carrierDict.Keys);

			var model = new CarrierPickerView(carriersList, 0);
			carrierPickerView.Model = model;

			UpdateContactButton.TouchUpInside += delegate
			{
				emergencyContact = new EmergencyContact(emergencyContact.ContactID, FirstNameTextField.Text, LastNameTextField.Text, PhoneNumberTextField.Text, EmailTextField.Text, carrierDict[model.getSelected()]);
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

			if (emergencyContact.ContactID == null)
			{
				UpdateContactButton.SetTitle("Add Contact", forState: UIControlState.Normal);
			}
			else
			{
				UpdateContactButton.SetTitle("Save Changes", forState: UIControlState.Normal);
			}

			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public async void UpdateContact(EmergencyContact emergencyContactIn)
		{
			if (emergencyContactIn.PhoneNumber.Length == 10)
			{
				//FIXME
				//update to userID
				if (await service.postContactToDatabase(emergencyContactIn, 1234) == 1)
				{
					emergencyContactsViewController.DismissUpdateContactViewModel();
				}
			}
			else
			{
				var alert = UIAlertController.Create("Invalid Contact", "This isn't a valid contact", UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
				PresentViewController(alert, true, null);
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
			//value = emergencyContact.Carrier;
			var index = carriersList.IndexOf(emergencyContact.Carrier);
			carrierPickerView.Select(index, 0, false);

		}
	}

	public partial class CarrierPickerView : UIPickerViewModel
	{
		List<string> _myItems;
		protected int selectedIndex = 0;
		string selectedCarrier;

		public CarrierPickerView(List<string> carriersList, int index)
		{
			_myItems = carriersList;
			selectedCarrier = _myItems[index];
		}

		public string SelectedItem
		{
			get { return selectedCarrier; }
		}

		public override nint GetComponentCount(UIPickerView picker)
		{
			return 1;
		}

		public override nint GetRowsInComponent(UIPickerView picker, nint component)
		{
			return _myItems.Count;
		}

		public override string GetTitle(UIPickerView picker, nint row, nint component)
		{
			return _myItems[(int)row];
			//return _myItems[(int)row];
		}

		public override void Selected(UIPickerView picker, nint row, nint component)
		{
			selectedIndex = (int)row;
			selectedCarrier = _myItems[(int)row];
		}

		public string getSelected()
		{
			return selectedCarrier;
		}

	}

}

