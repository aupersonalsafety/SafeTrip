
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;


namespace SafeTrip.Droid
{
	[Activity(Label = "EmergencyContactsActivity")]
	public class EmergencyContactsActivity : ListActivity
	{
		Service service = new Service();
		List<EmergencyContact> contacts;


		public string pin;
		public string userId;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			fetchContacts();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.AddMenu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public async Task fetchContacts()
		{
			contacts = await service.fetchContacts();

			ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, contacts.Select(x=>x.FirstName + " " + x.LastName).ToList());
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			base.OnListItemClick(l, v, position, id);

			Intent modify = new Intent(this, typeof(ModifyContactActivity));
			modify.PutExtra("firstName", contacts[position].FirstName);
			modify.PutExtra("lastName", contacts[position].LastName);
			if (contacts[position].contactEmail != null)
			{
				modify.PutExtra("email", contacts[position].contactEmail);
			}
			if (contacts[position].contactPhone != null)
			{
				modify.PutExtra("phoneNumber", contacts[position].contactPhone);
			}
			if (contacts[position].contactID != null)
			{
				modify.PutExtra("contactId", contacts[position].contactID ?? -1);
			}
			StartActivity(modify);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			//Open new contact activity
			switch (item.ItemId)
			{
				case Resource.Id.menu_add:
					StartActivity(typeof(ModifyContactActivity));
					break;
			}
			return base.OnOptionsItemSelected(item);
		}
	}
}
