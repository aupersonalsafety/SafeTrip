using Android.App;
using Android.OS;
using Android.Views;

namespace SafeTrip.Droid
{
	[Activity(Label = "ModifyContactActivity")]
	public class ModifyContactActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
		}

		public override View OnCreateView(string name, Android.Content.Context context, Android.Util.IAttributeSet attrs)
		{
			return base.OnCreateView(name, context, attrs);
		}

		public void choseFromAddressBook(View view)
		{
			StartActivity(typeof(AddressBookActivity));
		}
	}
}
