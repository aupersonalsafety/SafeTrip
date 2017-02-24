using System;
using System.Collections.Generic;

using UIKit;

using Plugin.Contacts;

namespace SafeTrip.iOS
{
	public partial class ContactsTableViewController : UITableViewController
	{

		public ContactsTableViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			//TableView.Source = new ContactsSource();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			Service service = new Service();
			List<Plugin.Contacts.Abstractions.Contact> tempContacts = service.getContacts().Result;

			TableView.Source = new ContactsSource(tempContacts);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			base.RowSelected(tableView, indexPath);
			tableView.DeselectRow(indexPath, true);
		}
	}

	public class ContactsSource : UITableViewSource
	{
		List<Plugin.Contacts.Abstractions.Contact> contacts;

		public ContactsSource()
		{
			contacts = new List<Plugin.Contacts.Abstractions.Contact>();
		}

		public ContactsSource(List<Plugin.Contacts.Abstractions.Contact> contactsIn)
		{
			contacts = contactsIn;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return contacts.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var CellIdentifier = "Cell";
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
			Plugin.Contacts.Abstractions.Contact item = contacts[indexPath.Row];

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

			cell.TextLabel.Text = item.DisplayName;

			return cell;
		}
	}
}

