using System;
using System.Threading.Tasks;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

using Auth0.SDK;

namespace SafeTrip.Droid
{
	[Activity(Label = "SafeTrip", MainLauncher = true, Icon = "@mipmap/icon")]

	public class MainActivity : Activity
	{
		Service service = new Service();
		const String PREF_NAME = "SafeTrip";
		String userId;
		String userToken;
		ISharedPreferences prefs;
		ISharedPreferencesEditor editor;

		private Auth0.SDK.Auth0Client client = new Auth0.SDK.Auth0Client("aupersonalsafety.auth0.com", "n4kXJEiHpBL3v1e0p0cM6pj8icidoZzo");


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.HomeScreen);

			prefs = GetSharedPreferences(PREF_NAME, FileCreationMode.Private);
			editor = prefs.Edit();

			setupUI();
			fetchLoginInfo();
		}

		public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.MainActivityMenu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.main_activity_menu:
					StartActivity(typeof(EmergencyContactsActivity));
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		private async Task fetchLoginInfo()
		{
			userId = prefs.GetString("userId", null);
			userToken = prefs.GetString("userToken", null);

			if (userId == null || userToken == null)
			{
				try 
				{
					var user = await this.client.LoginAsync(this);
					userToken = user.Auth0AccessToken;
					userId = user.Profile["user_id"].ToString();
					saveUserDetails();
				}
				catch (AggregateException e)
				{
					displayError(e.Flatten().Message);
				}
				catch (Exception e)
				{
					displayError(e.Message);
				}
			}
		}

		private void setupUI()
		{
			ImageButton panicButton = FindViewById<ImageButton>(Resource.Id.panicButton);
			panicButton.Click += delegate {
				
			};

			Button holdMyHandButton = FindViewById<Button>(Resource.Id.holdMyHandButton);
			holdMyHandButton.Click += delegate {
				
			};

			Button safeTripButton = FindViewById<Button>(Resource.Id.safeTripButton);
			safeTripButton.Click += delegate {
				
			};
		}

		public async Task UseTimer()
		{
			TextView timerText = FindViewById<TextView>(Resource.Id.timerText);
			int finished = await service.setTimer(10);
			if (finished == 1)
			{
				string works = "Works";
				timerText.SetText(works.ToCharArray(), 0, works.Length);
			}
			else
			{
				string works = "Doesnt Work";
				timerText.SetText(works.ToCharArray(), 0, works.Length);
			}
		}

		private void saveUserDetails()
		{
			editor.PutString("userId", userId);
			editor.PutString("userToken", userToken);
			editor.Apply();
		}

		private void displayError(String message)
		{
			Toast.MakeText(this, message, ToastLength.Short).Show();
		}
	}
}

