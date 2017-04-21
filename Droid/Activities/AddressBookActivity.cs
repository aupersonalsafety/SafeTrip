using System;
using System.Linq;
using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Widget;

namespace SafeTrip.Droid
{
	[Activity(Label = "AddressBookActivity")]
	public class AddressBookActivity : ListActivity
	{
		Service service = new Service();
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
		}

		public async Task fetchContacts()
		{
			ContactsList list = await service.getContacts();
			if (list.getError() == null)
			{
				ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, list.getContacts().Select(x => x.FirstName + " " + x.LastName).ToList());
			}
			else
			{
				Toast.MakeText(this, "Action selected: ",
					ToastLength.Short).Show();
			}
		}
	}
}
