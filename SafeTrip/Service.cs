using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
		public string userId;

		public Service()
		{
			userId = "-1";
			baseURI = "https://au-personal-safety.herokuapp.com/";
			sendSMSResourceURI = "email/sendtophone";
			helloResourceURI = "rest/greetings";
			positionResourceURI = "rest/position";
			emergencyContactResourceURI = "rest/emergecyContact";
		}

		public Service(String userIdIn)
		{
			userId = userIdIn;
			baseURI = "https://au-personal-safety.herokuapp.com/";
			sendSMSResourceURI = "email/sendtophone";
			helloResourceURI = "rest/greetings";
			positionResourceURI = "rest/position";
			emergencyContactResourceURI = "rest/emergecyContact";
		}

		public async Task<int> SendSMSMessage(string message, string recipientPhoneNumber)
		{
			String url = "https://au-personal-safety.herokuapp.com/email/sendtophone";

			var client = new HttpClient();

			Dictionary<String, Object> dict = new Dictionary<String, Object>();
			dict.Add("recipients", recipientPhoneNumber + "@vtext.com");
			dict.Add("subject", "none");
			dict.Add("messageText", message);
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

		public async Task<int> ContactEmergencyContacts()
		{

			String url = "https://au-personal-safety.herokuapp.com/alert/alertcontacts/" + userId;

			var client = new HttpClient();

			var response = await client.PostAsync(url, null);

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

			System.Diagnostics.Debug.WriteLine("globalPostion:D " + globalPosition);

			postLocationToDatabase(globalPosition, userId);
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

				postLocationToDatabase(globalPosition, userId);

				return globalPosition;
			}
		}

		public async Task<GlobalPosition> getLatLongFromAddress(String addressIn) 		{
			var key = "AIzaSyBwyE2TJ5l5VB-VygdMiWFB4kWPJj4WG58";

			var address = addressIn.Replace(" ", "+");
 			String url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=" + key;  			//var response = await  			//https://developer.xamarin.com/guides/xamarin-forms/web-services/consuming/rest/  			var client = new HttpClient(); 			client.MaxResponseContentBufferSize = 256000;  			var response = await client.GetAsync(url);  			if (response.IsSuccessStatusCode) 			{
				//response successful 				System.Diagnostics.Debug.WriteLine("response: " + response); 				//var json = Newtonsoft.Json.Linq.JObject.Parse(response.Content.ReadAsStringAsync().Result); 				var result = response.Content.ReadAsStringAsync().Result;
 				GlobalPosition globalPosition = getLatAndLongFromJson(result);
				return globalPosition; 			}
			else 
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful");
				return null;
			} 		}

		public GlobalPosition getLatAndLongFromJson(String result)
		{
			var json = JObject.Parse(result);
			//System.Diagnostics.Debug.WriteLine("json: " + json);
			System.Diagnostics.Debug.WriteLine("lat: " + json["results"][0]["geometry"]["location"]["lat"]);
			System.Diagnostics.Debug.WriteLine("long: " + json["results"][0]["geometry"]["location"]["lng"]);

			GlobalPosition globalPosition = new GlobalPosition();
			globalPosition.Latitude = (double)(json["results"][0]["geometry"]["location"]["lat"]);
			globalPosition.Longitude = (double)(json["results"][0]["geometry"]["location"]["lng"]);
			return globalPosition;
		}


		public async Task<int> getTravelTime(String addressIn)
		{

			var key = "AIzaSyAvljVjozDOssgpCqw-FP2VcqbJbQYVspA";

			var address = addressIn.Replace(" ", "+");

			GlobalPosition globPosition = await getGlobalPosition();

			String url = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + address +
				"&destinations=" + globPosition.Latitude + "," + globPosition.Longitude + "&key=" + key;

			//var response = await 
			//https://developer.xamarin.com/guides/xamarin-forms/web-services/consuming/rest/

			var client = new HttpClient();
			client.MaxResponseContentBufferSize = 256000;

			var response = await client.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				//response successful
				System.Diagnostics.Debug.WriteLine("response: " + response);
				//var json = Newtonsoft.Json.Linq.JObject.Parse(response.Content.ReadAsStringAsync().Result);
				var result = response.Content.ReadAsStringAsync().Result;
				return readTravelTime(result);
			}
			else 
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful");
				return -1;
			}

		} 		public int readTravelTime(string result)
		{
			var json = JObject.Parse(result);
			//System.Diagnostics.Debug.WriteLine("json: " + json);
			System.Diagnostics.Debug.WriteLine("timeEstimate: " + json["rows"][0]["elements"][0]["duration"]["value"]);
			return (int)(json["rows"][0]["elements"][0]["duration"]["value"]);
		}
 

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

		public async Task<List<EmergencyContact>> fetchContacts()
		{
			String url = "https://au-personal-safety.herokuapp.com/contact/getcontacts/" + userId;

			var client = new HttpClient();
			var response = await client.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				//response successful
				System.Diagnostics.Debug.WriteLine("response is successful: " + response.Content);
				return parseContactList(await response.Content.ReadAsStringAsync());
			}
			else
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful: " + response.Content);
				return new List<EmergencyContact>();
			}
		}

		//FIXME
		//parse this list
		private List<EmergencyContact> parseContactList(String contactsIn)
		{
			List<EmergencyContact> list = JsonConvert.DeserializeObject<List<EmergencyContact>>(contactsIn);
			return list;
		}

		public async Task<int> postContactToDatabase(EmergencyContact contact)
		{
			String url = "https://au-personal-safety.herokuapp.com/contact/sendtodb/" + userId;

			var client = new HttpClient();

			Dictionary<String, Object> dict = new Dictionary<String, Object>();
			dict.Add("FirstName", contact.FirstName);
			dict.Add("LastName", contact.LastName);
			dict.Add("ContactEmail", contact.contactEmail);
			dict.Add("ContactPhone", contact.contactPhone);
			dict.Add("ContactCarrier", contact.contactCarrier);
			dict.Add("ContactID", contact.contactID);

			//dict.Add("UserID", userId);
			string json = JsonConvert.SerializeObject(dict, Formatting.None);

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
				System.Diagnostics.Debug.WriteLine("response is successful: " + response.Content);
				return 1;
			}
			else
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful: " + await response.Content.ReadAsStringAsync());
				return -1;
			}
		}

		public async Task<int> deleteContactFromDatabase(int contactId)
		{
			String url = "https://au-personal-safety.herokuapp.com/contact/deletecontact";

			var client = new HttpClient();

			Dictionary<String, Object> dict = new Dictionary<String, Object>();
			dict.Add("contactID", contactId);
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

		public async Task<int> postLocationToDatabase(GlobalPosition position, string userID)
		{
			String url = "https://au-personal-safety.herokuapp.com/location/store/" + userID;

			var client = new HttpClient();

			Dictionary<String, Object> dict = new Dictionary<String, Object>();
			dict.Add("long", position.Longitude);
			dict.Add("lat", position.Latitude);
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
				System.Diagnostics.Debug.WriteLine("response is successful: " + response.Content);
				return 1;
			}
			else
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful: " + response.Content);
				return -1;
			}
		}
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
		}

		public async Task<string> getPin()
		{
			String url = "https://au-personal-safety.herokuapp.com/user/getPin/" + userId;

			var client = new HttpClient();

			var response = await client.PostAsync(url, null);

			if (response.IsSuccessStatusCode)
			{
				//response successful
				System.Diagnostics.Debug.WriteLine("response is successful: " + response.Content.ReadAsStringAsync().Result);
				return response.Content.ReadAsStringAsync().Result;
			}
			else
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful: " + response.Content);
				return "-1";
			}
		}

		public async Task<int> updatePin(String pin)
		{
			String url = "https://au-personal-safety.herokuapp.com/user/setPin/" + userId + "/" + pin;

			var client = new HttpClient();

			Dictionary<String, Object> dict = new Dictionary<String, Object>();
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
				System.Diagnostics.Debug.WriteLine("response is successful: " + response.Content);
				return 1;
			}
			else
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful: " + response.Content);
				return -1;
			}
		}

		//public async Task<int> extendServerTimer

		public async Task<int> setServerTimer(int seconds, string userID)
		{
			seconds = seconds + 60;
			String url = "https://au-personal-safety.herokuapp.com/starttimer/" + seconds + "/" + userID;

			var client = new HttpClient();

			Dictionary<String, Object> dict = new Dictionary<String, Object>();
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
				System.Diagnostics.Debug.WriteLine("response is successful: " + response.Content);
				return 1;
			}
			else
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful: " + response.Content);
				return -1;
			}
		}

		public async Task<int> createUser()
		{
			String url = "https://au-personal-safety.herokuapp.com/user/createuser/" + userId;

			var client = new HttpClient();

			Dictionary<String, Object> dict = new Dictionary<String, Object>();
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
				System.Diagnostics.Debug.WriteLine("response is successful: " + response.Content);
				return 1;
			}
			else
			{
				//reponse not successful
				System.Diagnostics.Debug.WriteLine("response is not successful: " + response.Content);
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
