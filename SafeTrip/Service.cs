using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Newtonsoft.Json.Linq;
using Plugin.Contacts;

namespace SafeTrip
{
    public class Service
    {
        private string baseURI;
        private string sendSMSResourceURI;
        private string helloResourceURI;

        public Service()
        {
            baseURI = "https://au-personal-safety.herokuapp.com/";
            sendSMSResourceURI = "rest/sms/send";
            helloResourceURI = "rest/greetings";
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
			GlobalPosition globalPosition = new GlobalPosition(position.Latitude, position.Longitude, position.Timestamp);
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
				var position = await locator.GetPositionAsync(timeoutMilliseconds: 200);
				globalPosition.Latitude = position.Latitude;
				globalPosition.Longitude = position.Longitude;
				return globalPosition;

			}
			catch (Exception ex)
			{
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
			} 		}  		public void handleData(String result) 		{ 			var json = JObject.Parse(result);
			//System.Diagnostics.Debug.WriteLine("json: " + json); 			System.Diagnostics.Debug.WriteLine("lat: " + json["results"][0]["geometry"]["location"]["lat"]);
			System.Diagnostics.Debug.WriteLine("long: " + json["results"][0]["geometry"]["location"]["lng"]); 		}

		public async void getContacts()
		{
			System.Diagnostics.Debug.WriteLine("getContacts called");
			if (await CrossContacts.Current.RequestPermission())
			{
				System.Diagnostics.Debug.WriteLine("requestPermission passed");
				List<Plugin.Contacts.Abstractions.Contact> contacts = null;
				CrossContacts.Current.PreferContactAggregation = false;//recommended
																	   //run in background
				await Task.Run(() =>
				{
					if (CrossContacts.Current.Contacts == null)
						return;

					contacts = CrossContacts.Current.Contacts.ToList();

					handleContacts(contacts);
				});
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("requestPermission failed");
			}
		}

		public void handleContacts(List<Plugin.Contacts.Abstractions.Contact> contacts)
		{
			System.Diagnostics.Debug.WriteLine("handleContacts called");
			contacts = contacts.Where(c => c.Phones.Count > 0).ToList();
			contacts = contacts.OrderBy(c => c.LastName).ToList();


			foreach (var contact in contacts)
			{
				//System.Diagnostics.Debug.WriteLine("contact: " + contact.Phones.FirstOrDefault().Number);
			}
		}
    }
}
