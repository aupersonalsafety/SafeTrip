﻿using System;
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

		public ModifyContactViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			var carrierDict = new Dictionary<String, String>();
			carrierDict.Add("AT&T", "@txt.att.net");
			carrierDict.Add("T-Mobile", "@tmomail.net");
			carrierDict.Add("Virgin Mobile", "@vmobl.com");
			carrierDict.Add("Cingular", "@cingularme.com");
			carrierDict.Add("Sprint", "@messaging.sprintpcs.com");
			carrierDict.Add("Verizon", "@vtext.com");
			carrierDict.Add("Nextel", "@messaging.nextel.com");

			var model = new CarrierPickerView(carrierDict, 0);
			carrierPickerView.Model = model;

			UpdateContactButton.TouchUpInside += delegate
			{
				emergencyContact = new EmergencyContact(emergencyContact.ContactID, FirstNameTextField.Text, LastNameTextField.Text, PhoneNumberTextField.Text, EmailTextField.Text, test.getSelected());
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
			//FIXME
			//update to userID
			if (await service.postContactToDatabase(emergencyContactIn, 1234) == 1)
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
			value = emergencyContact.Carrier;

		}
	}

	public partial class CarrierPickerView : UIPickerViewModel
	{
		Dictionary<String, String> _myItems;
		protected int selectedIndex = 0;
		string selectedCarrier;

		public CarrierPickerView(Dictionary<String, String> items, int index)
		{
			_myItems = items;
			selectedCarrier = new List<String>(_myItems.Keys)[0];
		}

		public string SelectedItem
		{
			get { return _myItems[selectedIndex]; }
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
		}

		public override void Selected(UIPickerView picker, nint row, nint component)
		{
			selectedIndex = (int)row;
			selectedCarrier = _myItems[selectedIndex];
		}

		public string getSelected()
		{
			return selectedCarrier;
		}

	}

}

