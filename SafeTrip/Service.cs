using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
    }
}
