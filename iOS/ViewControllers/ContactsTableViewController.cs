using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
			Title = "Contacts";
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			fetchContacts();
		}

		public async Task fetchContacts()
		{
			Service service = new Service();
			ContactsList contacts = await service.getContacts();

			if (contacts.getContacts() != null)
			{
				TableView.Source = new ContactsSource(contacts);
				TableView.ReloadData();
			}
			else
			{
				showAlert(contacts.getError());
			}
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

		public void showAlert(String error)
		{
			var alert = UIAlertController.Create("Error", error, UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
			PresentViewController(alert, true, null);
		}
	}

	public class ContactsSource : UITableViewSource
	{
		List<Plugin.Contacts.Abstractions.Contact> contacts;
		string[] keys;
		Dictionary<string, List<string>> indexedTableItems;

		public ContactsSource()
		{
			contacts = new List<Plugin.Contacts.Abstractions.Contact>();
		}

		public ContactsSource(ContactsList contactsListIn)
		{
			contacts = contactsListIn.getContacts();

			indexedTableItems = new Dictionary<string, List<string>>();
			foreach (var t in contacts)
			{
				if (indexedTableItems.ContainsKey(t.DisplayName[0].ToString()))
				{
					indexedTableItems[t.DisplayName[0].ToString()].Add(t.DisplayName);
				}
				else
				{
					indexedTableItems.Add(t.DisplayName[0].ToString(), new List<string>() { t.DisplayName });
				}
			}

			keys = indexedTableItems.Keys.ToArray();
			Array.Sort(keys);
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var CellIdentifier = "Cell";
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
			string item = indexedTableItems[keys[indexPath.Section]][indexPath.Row];

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

		public override nint NumberOfSections(UITableView tableView)
		{
			return keys.Length;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return indexedTableItems[keys[section]].Count;
		}

		public override string[] SectionIndexTitles(UITableView tableView)
		{
			return keys;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return keys[section];
		}

		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			return 1;
		}
	}
}

