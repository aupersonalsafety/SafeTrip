using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class ContactsTableViewController : UITableViewController
	{
		public ModifyContactViewController modifyContactsViewController;
		ContactsList contacts;
		public Service service;

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
			contacts = await service.getContacts();

			if (contacts.getContacts() != null)
			{
				TableView.Source = new ContactsSource(contacts, this);
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

		public void showAlert(String error)
		{
			var alert = UIAlertController.Create("Error", error, UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Cancel, null));
			PresentViewController(alert, true, null);
		}

		public void contactSelected(Plugin.Contacts.Abstractions.Contact contact)
		{
			modifyContactsViewController.DismissUpdateContactViewModel(contact);
		}
	}

	public class ContactsSource : UITableViewSource
	{
		List<Plugin.Contacts.Abstractions.Contact> contacts;
		string[] keys;
		Dictionary<string, List<Plugin.Contacts.Abstractions.Contact>> indexedTableItems;
		ContactsTableViewController owner;


		public ContactsSource(ContactsTableViewController ownerIn)
		{
			owner = ownerIn;
			contacts = new List<Plugin.Contacts.Abstractions.Contact>();
		}

		public ContactsSource(ContactsList contactsListIn, ContactsTableViewController ownerIn)
		{
			owner = ownerIn;
			contacts = contactsListIn.getContacts();

			indexedTableItems = new Dictionary<string, List<Plugin.Contacts.Abstractions.Contact>>();
			foreach (var t in contacts)
			{
				if (indexedTableItems.ContainsKey(t.DisplayName[0].ToString()))
				{
					indexedTableItems[t.DisplayName[0].ToString()].Add(t);
				}
				else
				{
					indexedTableItems.Add(t.DisplayName[0].ToString(), new List<Plugin.Contacts.Abstractions.Contact>() { t });
				}
			}

			keys = indexedTableItems.Keys.ToArray();
			Array.Sort(keys);
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var CellIdentifier = "Cell";
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
			string item = indexedTableItems[keys[indexPath.Section]][indexPath.Row].DisplayName;

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

			cell.TextLabel.Text = item;

			return cell;
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

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			owner.contactSelected(indexedTableItems[keys[indexPath.Section]][indexPath.Row]);
		}
	}
}

