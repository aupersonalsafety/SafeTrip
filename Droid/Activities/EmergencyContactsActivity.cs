
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;


namespace SafeTrip.Droid
{
	[Activity(Label = "EmergencyContactsActivity")]
	public class EmergencyContactsActivity : ListActivity
	{
		Service service = new Service();
		//List<Plugin.Contacts.Abstractions.Contact> contacts;
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
			List<EmergencyContact> list = await service.fetchContacts(1234);
			string[] arr = {"philip", "andrew"};
			//ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, list.Select(x=>x.FirstName + " " + x.LastName).ToList());
			ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, arr);
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			base.OnListItemClick(l, v, position, id);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			//Open new contact activity
			return base.OnOptionsItemSelected(item);
		}
	}
}
