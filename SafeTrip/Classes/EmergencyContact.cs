using System;
namespace SafeTrip
{
	public class EmergencyContact
	{
		public EmergencyContact()
		{
		}

		public EmergencyContact(int? contactIDIn, string firstName, string lastName, string phoneNumber, string email, string carrier)
		{
			contactID = contactIDIn;
			FirstName = firstName;
			LastName = lastName;
			contactPhone = phoneNumber;
			contactEmail = email;
			contactCarrier = carrier;
		}

		public int? contactID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string contactPhone { get; set;}
		public string contactEmail { get; set;}
		public string contactCarrier { get; set; }
	}
}
