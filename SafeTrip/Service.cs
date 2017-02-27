using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Newtonsoft.Json.Linq;
using Plugin.Contacts;
using Newtonsoft.Json;

namespace SafeTrip
{
    public class Service
    {
        private string baseURI;
        private string sendSMSResourceURI;
        private string helloResourceURI;
		private string positionResourceURI;
		private string emergencyContactResourceURI;

        public Service()
        {
            baseURI = "https://au-personal-safety.herokuapp.com/";
            sendSMSResourceURI = "rest/sms/send";
            helloResourceURI = "rest/greetings";
			positionResourceURI = "rest/position";
			emergencyContactResourceURI = "rest/emergecyContact";
        } 

        public async void SendSMSMessage(string message, string recipientPhoneNumber)
        {
            var values = new Dictionary<string, string>();
            values.Add("message", message);
            var content = new FormUrlEncodedContent(values);

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.PostAsync(baseURI + sendSMSResourceURI + "?message=" + message + "&phoneNumber=" + recipientPhoneNumber, content);

            response.EnsureSuccessStatusCode();
        }

        public async Task<String> SayHello(string name)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(baseURI + helloResourceURI + "/" + name);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

		public async Task<GlobalPosition> saveGlobalPosition()
		{
			GlobalPosition globalPosition = new GlobalPosition();
			try
			{

				System.Diagnostics.Debug.WriteLine("getGlobalPosition called");
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 10;

				System.Diagnostics.Debug.WriteLine("locator: {0}", locator);
				var position = await locator.GetPositionAsync(timeoutMilliseconds: 2000);
				globalPosition.Latitude = position.Latitude;
				globalPosition.Longitude = position.Longitude;

				string jsonPosition = Newtonsoft.Json.JsonConvert.SerializeObject(globalPosition);

				var client = new HttpClient();
				// Add body content
				var content = new StringContent(
					jsonPosition,
					Encoding.UTF8,
					"application/json"
				);

				// Send the request
				await client.PostAsync(baseURI + positionResourceURI + "/" + 1, content);
				return globalPosition;

			}
			catch (Exception ex)
			{
				//Fixme
				//This is temporary for testing on IOS emulator
				System.Diagnostics.Debug.WriteLine("Returning Dummy Location");
				System.Diagnostics.Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
				globalPosition.Latitude = 32.607722;
				globalPosition.Longitude = -85.489545;
				return globalPosition;
			}
		}

		public async void monitorLocation()
		{
			try {
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 1;

				locator.PositionChanged += positionChanged;
				locator.PositionError += positionErrorChanged;

				await locator.StartListeningAsync(1, 1);
			}
			catch(Exception ex)
			{
			  System.Diagnostics.Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
			}
		}

		public void positionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
		{
			var position = e.Position;
			GlobalPosition globalPosition = new GlobalPosition(position.Latitude, position.Longitude);
			//FIXME
			//call write to database with position?
		}

		public void positionErrorChanged(object sender, Plugin.Geolocator.Abstractions.PositionErrorEventArgs e)
		{
			var error = e.Error;
			System.Diagnostics.Debug.WriteLine("Position Error: {0}", error.ToString());
		}

		public async Task<GlobalPosition> getGlobalPosition()
		{
			GlobalPosition globalPosition = new GlobalPosition();
			try
			{
				
				System.Diagnostics.Debug.WriteLine("getGlobalPosition called");
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 10;

				System.Diagnostics.Debug.WriteLine("locator: {0}", locator);
				var position = await locator.GetPositionAsync(timeoutMilliseconds: 2000);
				globalPosition.Latitude = position.Latitude;
				globalPosition.Longitude = position.Longitude;
				return globalPosition;

			}
			catch (Exception ex)
			{
				//Fixme
				//This is temporary for testing on IOS emulator
				System.Diagnostics.Debug.WriteLine("Returning Dummy Location");
				System.Diagnostics.Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
				globalPosition.Latitude = 32.607722;
				globalPosition.Longitude = -85.489545;
				return globalPosition;
			}
		}

		public async void getLatLongFromAddress(String addressIn) 		{
			var key = "AIzaSyBwyE2TJ5l5VB-VygdMiWFB4kWPJj4WG58";

			var address = addressIn.Replace(" ", "+");
 			String url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=" + key;  			//var response = await  			//https://developer.xamarin.com/guides/xamarin-forms/web-services/consuming/rest/  			var client = new HttpClient(); 			client.MaxResponseContentBufferSize = 256000;  			var response = await client.GetAsync(url);  			if (response.IsSuccessStatusCode) 			{
				//response successful 				System.Diagnostics.Debug.WriteLine("response: " + response); 				//var json = Newtonsoft.Json.Linq.JObject.Parse(response.Content.ReadAsStringAsync().Result); 				var result = response.Content.ReadAsStringAsync().Result; 				handleData(result); 			}
			else 
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful");
			} 		}

		public async Task<int> setTimer(int seconds)
		{
			try
			{
				int milliseconds = seconds * 1000;
				await Task.Delay(milliseconds);
				return 1;
			}
			catch 
			{
				return -1;
			}
		}  		public void handleData(String result) 		{ 			var json = JObject.Parse(result);
			//System.Diagnostics.Debug.WriteLine("json: " + json); 			System.Diagnostics.Debug.WriteLine("lat: " + json["results"][0]["geometry"]["location"]["lat"]);
			System.Diagnostics.Debug.WriteLine("long: " + json["results"][0]["geometry"]["location"]["lng"]); 		}

		public async Task<ContactsList> getContacts()
		{
			System.Diagnostics.Debug.WriteLine("getContacts called");
			if (await CrossContacts.Current.RequestPermission())
			{
				System.Diagnostics.Debug.WriteLine("requestPermission passed");
				List<Plugin.Contacts.Abstractions.Contact> contacts = null;
				CrossContacts.Current.PreferContactAggregation = false;


				if (CrossContacts.Current.Contacts == null)
				{
					var contactsList = new ContactsList(null, "Error gettings contacts. Try again.");
					return contactsList;
				}
				else
				{
					contacts = CrossContacts.Current.Contacts.ToList();
					contacts = contacts.Where(c => c.Phones.Count > 0).ToList();
					contacts = contacts.OrderBy(c => c.LastName).ToList();
					var contactsList = new ContactsList(contacts, null);
					return contactsList;
				}
			}
			else
			{
				var contactsList = new ContactsList(null, "Contacts permission failed. Go to Settings to give SafeTrip access to your contacts.");
				return contactsList;
			}
		}

		public List<EmergencyContact> fetchContacts()
		{
			//dummy data
			List<EmergencyContact> list = new List<EmergencyContact>();
			EmergencyContact temp = new EmergencyContact(55, "Philip", "Sawyer", "555-555-5555", "phil@test.com");
			list.Add(temp);
			temp = new EmergencyContact(44, "Aaron", "Scherer", "444-444-4444", "aaron@test.com");
			list.Add(temp);
			return list;
		}

		public async Task<int> postContactToDatabase(EmergencyContact contact, int userId)
		{
			System.Diagnostics.Debug.WriteLine("postContactToDatabase called");
			String url = "https://au-personal-safety.herokuapp.com/contact/sendtodb";

			var client = new HttpClient();

			Dictionary<String, Object> dict = new Dictionary<String, Object>();
			dict.Add("firstName", contact.FirstName);
			dict.Add("lastName", contact.LastName);
			dict.Add("contactEmail", contact.Email);
			dict.Add("contactPhone", contact.PhoneNumber);
			//FIXME
			//Database needs to be able to update or new
			//dict.Add("contactID", contact.ContactID);
			dict.Add("userID", userId);
			var json = JsonConvert.SerializeObject(dict);

			System.Diagnostics.Debug.WriteLine("json: " + json);

			var content = new StringContent(
					json,
					Encoding.UTF8,
					"application/json"
				);

			var response = await client.PostAsync(url, content);

			if (response.IsSuccessStatusCode)
			{
				//response successful
				//System.Diagnostics.Debug.WriteLine("response is successful: " + response.Content);
				return 1;
			}
			else
			{
				//reponse not successful
				//System.Diagnostics.Debug.WriteLine("response is not successful: " + response.Content);
				return -1;
			}
		}
    }

	public class ContactsList
	{
		List<Plugin.Contacts.Abstractions.Contact> contacts;
		String errorString;

		public ContactsList(List<Plugin.Contacts.Abstractions.Contact> contactsIn, String errorStringIn)
		{
			contacts = contactsIn;
			errorString = errorStringIn;
		}

		public List<Plugin.Contacts.Abstractions.Contact> getContacts()
		{
			return contacts;
		}

		public String getError()
		{
			return errorString;
		}
	}
}
