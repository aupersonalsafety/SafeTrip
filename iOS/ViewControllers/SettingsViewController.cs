using System;

using UIKit;

namespace SafeTrip.iOS
{
	public partial class SettingsViewController : UITableViewController
	{
		string[] tableData;
		public SettingsViewController() : base("SettingsViewController", null)
		{
		}

		public SettingsViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			Title = "Settings";

			tableData = getTableData();
			TableView.Source = new TableSource(tableData);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		private string[] getTableData()
		{
			return new String[] { "Emergency Contacts", "Update Pin Number", "Sign Out" };
		}

		public void openEmergencyContacts()
		{

		}

		public void updatePinNumber()
		{

		}

		public void signOut()
		{

		}
	}

	public class TableSource : UITableViewSource
	{
		string[] TableItems;
		string CellIdentifier = "TableCell";
		SettingsViewController owner;

		public TableSource(string[] items, SettingsViewController ownerIn)
		{
			TableItems = items;
			owner = ownerIn;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return 1;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return TableItems.Length;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
			string item = TableItems[indexPath.Section];

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

			cell.TextLabel.Text = item;
			cell.TextLabel.TextAlignment = UITextAlignment.Center;

			if (indexPath.Section == 2)
			{
				cell.TextLabel.TextColor = UIColor.Red;
			}

			return cell;
		}

		public override nfloat GetHeightForFooter(UITableView tableView, nint section)
		{
			return 10;
		}

		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			return 10;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);

			switch (indexPath.Row)
			{
				case 0:
					owner.openEmergencyContacts();
					break;
				case 1:
					owner.updatePinNumber();
					break;
				case 2:
					owner.signOut();
					break;
			}
		}
	}
}