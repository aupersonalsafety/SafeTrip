using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Contacts.Abstractions;

using BigTed;

using UIKit;
using System.Text.RegularExpressions;

using Foundation;

namespace SafeTrip.iOS
{
	public partial class ModifyContactViewController : UIViewController
	{
		public EmergencyContactsViewController emergencyContactsViewController;
		public string userId;
		public EmergencyContact emergencyContact;
		public Auth0.SDK.Auth0Client client;

		public Service service;
		Dictionary<string, string> carrierDict;
		List<string> carriersList;
		int? emergencyContactID;

		public ModifyContactViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

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

			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem("Address Book", UIBarButtonItemStyle.Plain, (sender, args) =>
				{
					var storyBoard = UIStoryboard.FromName("ContactsSelector", null);
					ContactsTableViewController contactsTableViewController = (ContactsTableViewController)storyBoard.InstantiateViewController("ContactsTableViewController");
					contactsTableViewController.modifyContactsViewController = this;
					contactsTableViewController.service = service;
					if (contactsTableViewController != null)
					{
						NavigationController.PushViewController(contactsTableViewController, true);
					}
				})
			, true);


			UpdateContactButton.TouchUpInside += delegate
			{
				bool valid = true;
				string phoneNumber = PhoneNumberTextField.Text;
				long phoneNumberInt;
				if (Int64.TryParse(phoneNumber, out phoneNumberInt))
				{
					if (phoneNumber.Length == 11)
					{
						if (phoneNumber[0] == '1')
						{
							phoneNumber = phoneNumber.Substring(1);
						}
						else
						{
							valid = false;
						}
					}
					else if (phoneNumber.Length != 10)
					{
						valid = false;
					}
				}
				else
				{
					valid = false;	
				}
				if (valid)
				{
					emergencyContact = new EmergencyContact(emergencyContact.contactID, FirstNameTextField.Text, LastNameTextField.Text, phoneNumber, EmailTextField.Text, carrierDict[model.getSelected()]);
					UpdateContact(emergencyContact);
				}
				else
				{
					var alert = UIAlertController.Create("Error", "This is not a valid phonenumber. Please enter a valid phone number to continue.", UIAlertControllerStyle.Alert);
					alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
					PresentViewController(alert, true, null);
				}

			};

			LoadEmergencyContact(emergencyContact);

			if (emergencyContact.contactID == null)
			{
				UpdateContactButton.SetTitle("Add Contact", forState: UIControlState.Normal);
			}
			else
			{
				UpdateContactButton.SetTitle("Save Changes", forState: UIControlState.Normal);
			}


			FirstNameTextField.ShouldReturn += (textField) =>
			{
				((UITextField)textField).ResignFirstResponder();
				return true;
			};

			LastNameTextField.ShouldReturn += (textField) =>
			{
				((UITextField)textField).ResignFirstResponder();
				return true;
			};

			PhoneNumberTextField.ShouldReturn += (textField) =>
			{
				((UITextField)textField).ResignFirstResponder();
				return true;
			};

			EmailTextField.ShouldReturn += (textField) =>
			{
				((UITextField)textField).ResignFirstResponder();
				return true;
			};
		}

		public async Task UpdateContact(EmergencyContact emergencyContactIn)
		{
			if (emergencyContactIn.contactPhone.Length == 10)
			{
				BTProgressHUD.Show(status: "Loading...");
				if (await service.postContactToDatabase(emergencyContactIn) == 1)
				{
					BTProgressHUD.Dismiss();
					emergencyContactsViewController.DismissUpdateContactViewModel();
				}
				else
				{
					BTProgressHUD.Dismiss();
					var alert = UIAlertController.Create("Error", "Could not save contact", UIAlertControllerStyle.Alert);
					alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
					PresentViewController(alert, true, null);
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
				PhoneNumberTextField.Text = removeLetters(contact.Phones[0].Number);
			}
			if (contact.Emails.Count > 0)
			{
				EmailTextField.Text = contact.Emails[0].Address;
			}
		}

		public void LoadEmergencyContact(EmergencyContact emergencyContact)
		{
			emergencyContactID = emergencyContact.contactID;
			FirstNameTextField.Text = emergencyContact.FirstName;
			LastNameTextField.Text = emergencyContact.LastName;
			PhoneNumberTextField.Text = removeLetters(emergencyContact.contactPhone);
			EmailTextField.Text = emergencyContact.contactEmail;
			var index = carriersList.IndexOf(emergencyContact.contactCarrier);
			carrierPickerView.Select(index, 0, false);
		}

		private String removeLetters(String phoneNumber)
		{
			string resultString = null;
			try
			{
				Regex regexObj = new Regex(@"[^\d]");
				resultString = regexObj.Replace(phoneNumber, "");
			}
			catch (ArgumentException ex)
			{
				// Syntax error in the regular expression
			}

			return resultString;
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

	public partial class NumberOnlyTextField : UITextFieldDelegate
	{
		const int maxCharacters = 10;
		public override bool ShouldChangeCharacters(UITextField textField, Foundation.NSRange range, string replacement)
		{
			var newContent = new NSString(textField.Text).Replace(range, new NSString(replacement)).ToString();
			int number;
			return newContent.Length <= maxCharacters && (replacement.Length == 0 || int.TryParse(replacement, out number));
		}
	}

}

