using System;

using Android.App;
using Android.Widget;
using Android.OS;

namespace SafeTrip.Droid
{
	[Activity(Label = "SafeTrip", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		Service service = new Service();

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.HomeScreen);

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

		public async void UseTimer()
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
	}
}

