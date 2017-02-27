using System;
namespace SafeTrip
{
	public class EmergencyContact
	{
		public EmergencyContact()
		{
		}

		public EmergencyContact(int? contactID, string firstName, string lastName, string phoneNumber, string email)
		{
			ContactID = contactID;
			FirstName = firstName;
			LastName = lastName;
			PhoneNumber = phoneNumber;
			Email = email;
		}

		public int? ContactID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set;}
		public string Email { get; set;}
	}
}
