using System;
using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace SafeTrip.Droid
{
	[Activity(Label = "ModifyContactActivity")]
	public class ModifyContactActivity : Activity
	{
		EditText firstNameEditText;
		EditText lastNameEditText;
		EditText phoneNumberEditText;
		EditText emailEditText;
		Button saveContactButton;
		EmergencyContact emergencyContact;
		Service service = new Service();

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.ModifyContactLayout);

			Button addressBookButton = FindViewById<Button>(Resource.Id.addFromAddressBookButton);
			addressBookButton.Click += delegate
			{
				choseFromAddressBook();
			};
			firstNameEditText = FindViewById<EditText>(Resource.Id.firstNameEditText);
			lastNameEditText = FindViewById<EditText>(Resource.Id.lastNameEditText);
			phoneNumberEditText = FindViewById<EditText>(Resource.Id.phoneNumberEditText);
			emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
			saveContactButton = FindViewById<Button>(Resource.Id.saveContactButton);

			emergencyContact = new EmergencyContact();
			if (Intent.HasExtra("firstName"))
			{
				emergencyContact.FirstName = Intent.GetStringExtra("firstName");
			}
			if (Intent.HasExtra("lastName"))
			{
				emergencyContact.LastName = Intent.GetStringExtra("lastName");
			}
			if (Intent.HasExtra("phoneNumber"))
			{
				emergencyContact.PhoneNumber = Intent.GetStringExtra("phoneNumber");
			}
			if (Intent.HasExtra("email"))
			{
				emergencyContact.Email = Intent.GetStringExtra("email");
			}
			if (Intent.HasExtra("contactId"))
			{
				int contactId = Intent.GetIntExtra("contactId", -1);
				if (contactId == -1)
				{
					emergencyContact.ContactID = null;
				}
				else
				{
					emergencyContact.ContactID = contactId;
				}
			}
			else
			{
				emergencyContact.ContactID = null;
			}

			insertUser();

			saveContactButton.Click += delegate {
				emergencyContact = new EmergencyContact(emergencyContact.ContactID, firstNameEditText.Text, lastNameEditText.Text, phoneNumberEditText.Text, emailEditText.Text, "Verizon");
				UpdateContact(emergencyContact);
			};
		}

		public override View OnCreateView(string name, Android.Content.Context context, Android.Util.IAttributeSet attrs)
		{
			return base.OnCreateView(name, context, attrs);
		}

		public void choseFromAddressBook()
		{
			var addressBook = new Intent(this, typeof(AddressBookActivity));
			StartActivityForResult(addressBook, 0);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (resultCode == Result.Ok)
			{
				emergencyContact.FirstName = data.GetStringExtra("firstName");
				emergencyContact.LastName = data.GetStringExtra("lastName");
				emergencyContact.PhoneNumber = data.GetStringExtra("phoneNumber");
				emergencyContact.Email = data.GetStringExtra("email");
				emergencyContact.ContactID = null;

				insertUser();
			}
		}

		private void insertUser()
		{
			firstNameEditText.Text = emergencyContact.FirstName;
			lastNameEditText.Text = emergencyContact.LastName;
			phoneNumberEditText.Text = emergencyContact.PhoneNumber;
			emailEditText.Text = emergencyContact.Email;
			if (emergencyContact.ContactID == null)
			{
				saveContactButton.Text = "Save Contact";
			}
			else
			{
				saveContactButton.Text = "Update Contact";
			}
		}

		private async void UpdateContact(EmergencyContact emergencyContactIn)
		{
			if (emergencyContactIn.PhoneNumber.Length == 10)
			{
				//FIXME
				//update to userID
				AndroidHUD.AndHUD.Shared.Show(this, "Loading", maskType: AndroidHUD.MaskType.Clear);
				if (await service.postContactToDatabase(emergencyContactIn, 1) == 1)
				{
					AndroidHUD.AndHUD.Shared.Dismiss(this);
					Finish();
				}
				else
				{
					AndroidHUD.AndHUD.Shared.Dismiss(this);
					displayError("Error, could not save contact. Please try again.");
				}
			}
			else
			{
				displayError("Error: This is not a valid contact. Please try again.");
			}
		}

		private void displayError(String error)
		{
			Toast.MakeText(this, error, ToastLength.Short).Show();
		}
	}
}
