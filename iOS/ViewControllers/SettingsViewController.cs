using System;
using System.Threading.Tasks;

using UIKit;
using Foundation;
using BigTed;

using Auth0.SDK;

namespace SafeTrip.iOS
{
	public partial class SettingsViewController : UITableViewController
	{
		string[] tableData;

		public string pin;
		public Auth0Client client;
		public ViewController presentingViewController;
		public string userId;

		public Service service;

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
			TableView.Source = new TableSource(tableData, this);
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
			var storyBoard = UIStoryboard.FromName("EmergencyContactsMenu", Foundation.NSBundle.MainBundle);
			EmergencyContactsViewController emergencyContactsVC = (EmergencyContactsViewController)storyBoard.InstantiateViewController("EmergencyContactsViewController");
			emergencyContactsVC.userId = userId;
			emergencyContactsVC.client = client;
			emergencyContactsVC.service = service;
			if (emergencyContactsVC != null)
			{
				NavigationController.PushViewController(emergencyContactsVC, true);
			}
		}

		public void updatePinNumber()
		{
			UIAlertController alert = UIAlertController.Create("Update Pin", "Please enter your old pin and confirm your new pin numbers. Pins are 4 numbers", UIAlertControllerStyle.Alert);
			string oldPin = "";
			string newPin = "";
			string confirmNewPin = "";
			alert.AddTextField((UITextField obj) =>
			{
				obj.Placeholder = "Enter old pin";
				obj.SecureTextEntry = true;
				obj.KeyboardType = UIKeyboardType.NumberPad;

				obj.EditingChanged += delegate
				{
					oldPin = obj.Text;
				};

				obj.ShouldChangeCharacters = (UITextField textField, NSRange range, string replacementString) =>
				{
					var length = textField.Text.Length - range.Length + replacementString.Length;
					return length <= 4;
				};
			});

			alert.AddTextField((obj) =>
			{
				obj.Placeholder = "Enter new pin";
				obj.SecureTextEntry = true;
				obj.KeyboardType = UIKeyboardType.NumberPad;

				obj.EditingChanged += delegate {
					newPin = obj.Text;
				};

				obj.ShouldChangeCharacters = (UITextField textField, NSRange range, string replacementString) =>
				{
					var length = textField.Text.Length - range.Length + replacementString.Length;
					return length <= 4;
				};
			});

			alert.AddTextField((UITextField obj) =>
			{
				obj.Placeholder = "Confirm new pin";
				obj.SecureTextEntry = true;
				obj.KeyboardType = UIKeyboardType.NumberPad;

				obj.EditingChanged += delegate {
					confirmNewPin = obj.Text;
				};

				obj.ShouldChangeCharacters = (UITextField textField, NSRange range, string replacementString) =>
				{
					var length = textField.Text.Length - range.Length + replacementString.Length;
					return length <= 4;
				};
			});

			var cancelAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null);
			var updateAction = UIAlertAction.Create("Update", UIAlertActionStyle.Default, (obj) =>
			{
				if (oldPin.Equals(pin) && newPin.Equals(confirmNewPin))
				{
					updatePin(newPin);
				}
				else
				{
					updatePinNumber();
				}
			});

			alert.AddAction(cancelAction);
			alert.AddAction(updateAction);

			PresentViewController(alert, true, null);
		}

		private async Task updatePin(string newPin)
		{
			BTProgressHUD.Show(status: "Loading...");
			await service.updatePin(newPin);
			BTProgressHUD.Dismiss();

			pin = newPin;

			presentingViewController.updatePin(newPin);
		}

		public void signOut()
		{
			var defaults = NSUserDefaults.StandardUserDefaults;
			defaults.RemoveObject("userId");
			defaults.RemoveObject("userToken");
			defaults.Synchronize();
			client.Logout();
			presentingViewController.dismissSettings();
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

			switch (indexPath.Section)
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