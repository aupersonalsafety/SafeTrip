using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Foundation;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class EmergencyContactsViewController : UITableViewController
	{
		Service service;
		public string userId;
		public Auth0.SDK.Auth0Client client;
		public ViewController presentingViewController;

		public EmergencyContactsViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			service = new Service();

			Title = "Emergency Contacts";

			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, args) =>
				{
					var storyBoard = UIStoryboard.FromName("ModifyContact", Foundation.NSBundle.MainBundle);
					ModifyContactViewController modifyContactViewController = (ModifyContactViewController)storyBoard.InstantiateViewController("ModifyContactViewController");
					modifyContactViewController.client = client;
					modifyContactViewController.userId = userId;
					if (modifyContactViewController != null)
					{
						modifyContactViewController.userId = userId;
						modifyContactViewController.emergencyContact = new EmergencyContact();
						modifyContactViewController.emergencyContactsViewController = this;
						NavigationController.PushViewController(modifyContactViewController, true);
					}
				})
			, true);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			refreshContacts();
		}

		public void DismissUpdateContactViewModel()
		{
			NavigationController.PopViewController(true);
		}

		public void contactSelected(EmergencyContact emergencyContact)
		{
			var storyBoard = UIStoryboard.FromName("ModifyContact", Foundation.NSBundle.MainBundle);
			ModifyContactViewController modifyContactViewController = (ModifyContactViewController)storyBoard.InstantiateViewController("ModifyContactViewController");

			if (modifyContactViewController != null)
			{
				modifyContactViewController.emergencyContact = new EmergencyContact();
				modifyContactViewController.emergencyContact.ContactID = emergencyContact.ContactID;
				modifyContactViewController.emergencyContact.FirstName = emergencyContact.FirstName;
				modifyContactViewController.emergencyContact.LastName = emergencyContact.LastName;
				modifyContactViewController.emergencyContact.PhoneNumber = emergencyContact.PhoneNumber;
				modifyContactViewController.emergencyContact.Email = emergencyContact.Email;
				modifyContactViewController.emergencyContactsViewController = this;
				NavigationController.PushViewController(modifyContactViewController, true);
				//modifyContactViewController.LoadEmergencyContact(modifyContactViewController.emergencyContact);
				
			}
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}

		public async Task refreshContacts()
		{
			var fetchedContacts = await service.fetchContacts(userId);
			TableView.Source = new EmergencyContactsDataSource(fetchedContacts, this);
		}

		public async Task removeContact(int contactId)
		{
			if (await service.deleteContactFromDatabase(contactId) == -1)
			{
				await refreshContacts();
			}
		}
	}

	public class EmergencyContactsDataSource : UITableViewSource
	{
		List<EmergencyContact> contacts;
		EmergencyContactsViewController owner;

		public EmergencyContactsDataSource(List<EmergencyContact> contactsIn, EmergencyContactsViewController emergencyContactsViewController)
		{
			contacts = contactsIn;
			owner = emergencyContactsViewController;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return contacts.Count;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var CellIdentifier = "Cell";
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
			string item = contacts[indexPath.Row].FirstName + " " + contacts[indexPath.Row].LastName;

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

			cell.TextLabel.Text = item;

			return cell;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			owner.contactSelected(contacts[indexPath.Row]);
		}

		public override bool CanEditRow(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			//return base.CanEditRow(tableView, indexPath);
			return true;
		}

		public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
		{
			switch (editingStyle)
			{
				case UITableViewCellEditingStyle.Delete:
					// remove the item from the underlying data source
					int contactId = (int)contacts[indexPath.Row].ContactID;
					owner.removeContact(contactId);
					contacts.RemoveAt(indexPath.Row);
					// delete the row from the table
					tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
					break;
				case UITableViewCellEditingStyle.None:
					Console.WriteLine("CommitEditingStyle:None called");
					break;
			}
		}
	}
}
