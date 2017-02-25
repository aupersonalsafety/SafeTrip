using System;
using System.Collections.Generic;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class EmergencyContactsViewController : UITableViewController
	{
		public EmergencyContactsViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			Title = "Emergency Contacts";

			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, args) =>
				{
					var storyBoard = UIStoryboard.FromName("ModifyContact", Foundation.NSBundle.MainBundle);
					ModifyContactViewController modifyContactViewController = (ModifyContactViewController)storyBoard.InstantiateViewController("ModifyContactViewController");

					if (modifyContactViewController != null)
					{
						modifyContactViewController.emergencyContact = new EmergencyContact();
						modifyContactViewController.emergencyContactsViewController = this;
						NavigationController.PushViewController(modifyContactViewController, true);
					}
				})
			, true);

			Service service = new Service();
			TableView.Source = new EmergencyContactsDataSource(service.fetchContacts());
		}

		public void DismissUpdateContactViewModel()
		{
			NavigationController.PopViewController(true);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.		
		}
	}

	public class EmergencyContactsDataSource : UITableViewSource
	{
		List<EmergencyContact> contacts;
		public EmergencyContactsDataSource(List<EmergencyContact> contactsIn)
		{
			contacts = contactsIn;
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
		}
	}
}
