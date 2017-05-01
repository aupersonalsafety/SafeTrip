using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Text;

using Auth0.SDK;

namespace SafeTrip.Droid
{
	[Activity(Label = "SettingsActivity")]
	public class SettingsActivity : Activity
	{
		private Auth0Client client = new Auth0Client("aupersonalsafety.auth0.com", "n4kXJEiHpBL3v1e0p0cM6pj8icidoZzo");
		Service service;
		string userId;
		String pin;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.SettingsLayout);

			userId = Intent.GetStringExtra("userId");
			pin = Intent.GetStringExtra("pin");

			service = new Service(userId);

			setupUI();
		}

		private void setupUI()
		{
			Button emergencyContactsButton = (Button) FindViewById(Resource.Id.settings_emergency_contacts_button);
			emergencyContactsButton.Click += delegate
			{
				var emergencyContact = new Intent(this, typeof(EmergencyContactsActivity));
				emergencyContact.PutExtra("userId", userId);
				StartActivity(emergencyContact);
			};

			Button updatePinButton = (Button) FindViewById(Resource.Id.settings_update_pin_number_button);
			updatePinButton.Click += delegate 
			{
				displayPinAlert();
			};

			Button signOutButton = (Button)FindViewById(Resource.Id.settings_sign_out_button);
			signOutButton.Click += delegate {
				client.Logout();
				ISharedPreferencesEditor editor = GetSharedPreferences(Constants.PREF_NAME, FileCreationMode.Private).Edit();
				editor.PutString("userId", null);
				editor.PutString("userToken", null);
				editor.Apply();

				Intent myIntent = new Intent(this, typeof(MainActivity));
				myIntent.PutExtra ("signOut", true);
				SetResult(Result.Ok, myIntent);
				Finish();
			};
		}

		private void displayPinAlert()
		{
			AlertDialog.Builder alert = new AlertDialog.Builder(this);
			alert.SetTitle("Update Pin");

			var lengthFilter = new IInputFilter[] { new InputFilterLengthFilter(4) };

			LinearLayout layout = new LinearLayout(this);
			layout.Orientation = Orientation.Vertical;
			EditText oldPinEditText = new EditText(this);
			oldPinEditText.InputType = InputTypes.ClassNumber | InputTypes.NumberVariationPassword;
			oldPinEditText.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.WrapContent);
			oldPinEditText.Hint = "Enter Old Pin";
			oldPinEditText.SetFilters(lengthFilter);

			layout.AddView(oldPinEditText);

			EditText newPinEditText = new EditText(this);
			newPinEditText.InputType = InputTypes.ClassNumber | InputTypes.NumberVariationPassword;
			newPinEditText.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.WrapContent);
			newPinEditText.Hint = "Enter New Pin";
			newPinEditText.SetFilters(lengthFilter);
			newPinEditText.InputType = InputTypes.NumberVariationPassword;
			layout.AddView(newPinEditText);

			EditText confirmNewPinEditText = new EditText(this);
			confirmNewPinEditText.InputType = InputTypes.ClassNumber | InputTypes.NumberVariationPassword;
			confirmNewPinEditText.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.WrapContent);
			confirmNewPinEditText.Hint = "Confirm New Pin";
			confirmNewPinEditText.SetFilters(lengthFilter);
			confirmNewPinEditText.InputType = InputTypes.NumberVariationPassword;
			layout.AddView(confirmNewPinEditText);

			alert.SetView(layout);

			alert.SetNegativeButton("Cancel", (sender, e) =>
			{
				
			});

			alert.SetPositiveButton("Done", (sender, e) =>
			{
				var oldPin = oldPinEditText.Text;
				var newPin = newPinEditText.Text;
				var confrimNewPin = confirmNewPinEditText.Text;

				if (oldPin.Equals(pin) && newPin.Equals(confrimNewPin))
				{
					updatePin(newPin);
				}
				else
				{
					displayPinAlert();
				}

			});
			AlertDialog dialog = alert.Create();
			dialog.Show();
		}

		private async Task updatePin(String newPin)
		{
			pin = newPin;
			AndroidHUD.AndHUD.Shared.Show(this, "Loading", maskType: AndroidHUD.MaskType.Clear);
			await service.updatePin(newPin);
			AndroidHUD.AndHUD.Shared.Dismiss();

			Intent myIntent = new Intent(this, typeof(MainActivity));
			myIntent.PutExtra ("pinUpdated", true);
			SetResult(Result.Ok, myIntent);
			Finish();
		}
	}
}
