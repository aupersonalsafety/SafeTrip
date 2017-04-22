using System;

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

			if (Intent.HasExtra("firstName"))
			{
				firstNameEditText.Text = Intent.GetStringExtra("firstName");
			}
			if (Intent.HasExtra("lastName"))
			{
				lastNameEditText.Text = Intent.GetStringExtra("lastName");
			}
			if (Intent.HasExtra("phoneNumber"))
			{
				phoneNumberEditText.Text = Intent.GetStringExtra("phoneNumber");
			}
			if (Intent.HasExtra("email"))
			{
				emailEditText.Text = Intent.GetStringExtra("email");
			}
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
				firstNameEditText.Text = data.GetStringExtra("firstName");
				lastNameEditText.Text = data.GetStringExtra("lastName");
				phoneNumberEditText.Text = data.GetStringExtra("phoneNumber");
				emailEditText.Text = data.GetStringExtra("email");
			}
		}
	}
}
