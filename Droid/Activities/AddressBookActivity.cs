using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;

namespace SafeTrip.Droid
{
	[Activity(Label = "AddressBookActivity")]
	public class AddressBookActivity : ListActivity
	{
		Service service = new Service();
		List<Plugin.Contacts.Abstractions.Contact> contacts;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			fetchContacts();
		}

		public async Task fetchContacts()
		{
			ContactsList list = await service.getContacts();
			if (list.getError() == null)
			{
				contacts = list.getContacts();
				ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, contacts.Select(x => x.FirstName + " " + x.LastName).ToList());
			}
			else
			{
				Toast.MakeText(this, "Action selected: ",
					ToastLength.Short).Show();
			}
		}

		protected override void OnListItemClick(ListView l, Android.Views.View v, int position, long id)
		{
			base.OnListItemClick(l, v, position, id);
			Plugin.Contacts.Abstractions.Contact selection = contacts[position];
			Intent result = new Intent(this, typeof(AddressBookActivity));
			result.PutExtra("firstName", selection.FirstName);
			result.PutExtra("lastName", selection.LastName);
			if (selection.Phones.Count > 0)
			{
				result.PutExtra("phoneNumber", selection.Phones.First().Number);
			}
			if (selection.Emails.Count > 0)
			{
				result.PutExtra("email", selection.Emails.First().Label);
			}
			SetResult(Result.Ok, result);
			Finish();
		}
	}
}
