using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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
		Service service;

		String userId;
		String selectedCarrier;

		Dictionary<string, string> carrierDict;
		List<string> carriersList;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.ModifyContactLayout);

			userId = Intent.GetStringExtra("userId");

			service = new Service(userId);



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
				emergencyContact.contactPhone = Intent.GetStringExtra("phoneNumber");
			}
			if (Intent.HasExtra("email"))
			{
				emergencyContact.contactEmail = Intent.GetStringExtra("email");
			}
			if (Intent.HasExtra("contactId"))
			{
				int contactId = Intent.GetIntExtra("contactId", -1);
				if (contactId == -1)
				{
					emergencyContact.contactID = null;
				}
				else
				{
					emergencyContact.contactID = contactId;
				}
			}
			else
			{
				emergencyContact.contactID = null;
			}

			saveContactButton.Click += delegate
			{
				emergencyContact = new EmergencyContact(emergencyContact.contactID, firstNameEditText.Text, lastNameEditText.Text, phoneNumberEditText.Text, emailEditText.Text, selectedCarrier);
				UpdateContact(emergencyContact);
			};

			carrierDict = new Dictionary<string, string>();
			carrierDict.Add("AT&T", "@txt.att.net");
			carrierDict.Add("T-Mobile", "@tmomail.net");
			carrierDict.Add("Virgin Mobile", "@vmobl.com");
			carrierDict.Add("Cingular", "@cingularme.com");
			carrierDict.Add("Sprint", "@messaging.sprintpcs.com");
			carrierDict.Add("Verizon", "@vtext.com");
			carrierDict.Add("Nextel", "@messaging.nextel.com");

			carriersList = new List<string>();
			carriersList.AddRange(carrierDict.Keys);

			Spinner carSpin = FindViewById<Spinner>(Resource.Id.carrierSelector);
			carSpin.ItemSelected += spinner_ItemSelected;

			var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, carriersList);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			carSpin.Adapter = adapter;

			insertUser();
		}

		private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			var selected = carriersList[e.Position];

			selectedCarrier = carrierDict[selected];
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
				emergencyContact.contactPhone = data.GetStringExtra("phoneNumber");
				emergencyContact.contactEmail = data.GetStringExtra("email");
				emergencyContact.contactID = null;

				insertUser();
			}
		}

		private void insertUser()
		{
			firstNameEditText.Text = emergencyContact.FirstName;
			lastNameEditText.Text = emergencyContact.LastName;
			phoneNumberEditText.Text = emergencyContact.contactPhone;
			emailEditText.Text = emergencyContact.contactEmail;
			if (emergencyContact.contactID == null)
			{
				saveContactButton.Text = "Save Contact";
			}
			else
			{
				saveContactButton.Text = "Update Contact";
			}
		}

		private async Task UpdateContact(EmergencyContact emergencyContactIn)
		{
			if (emergencyContactIn.contactPhone.Length == 10)
			{
				AndroidHUD.AndHUD.Shared.Show(this, "Loading", maskType: AndroidHUD.MaskType.Clear);
				if (await service.postContactToDatabase(emergencyContactIn) == 1)
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
