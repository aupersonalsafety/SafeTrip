using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;

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
			System.Diagnostics.Debug.WriteLine("Position Status: {0}", position.Timestamp);
			System.Diagnostics.Debug.WriteLine("Position Latitude: {0}", position.Latitude);
			System.Diagnostics.Debug.WriteLine("Position Longitude: {0}", position.Longitude);
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
				
				System.Diagnostics.Debug.WriteLine("getLatitude called");
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 10;

				System.Diagnostics.Debug.WriteLine("locator: {0}", locator);

				var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

				globalPosition.Latitude = position.Latitude;
				globalPosition.Longitude = position.Longitude;

				return globalPosition;

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
				return globalPosition;
			}
		}
    }
}
