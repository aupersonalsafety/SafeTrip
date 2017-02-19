using System;
namespace SafeTrip
{
	public class GlobalPosition
	{
		public GlobalPosition()
		{
		}

		public GlobalPosition(double lat, double longitude, DateTimeOffset time)
		{
			Latitude = lat;
			Longitude = longitude;
			timestamp = time;
		}

		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DateTimeOffset timestamp {get; set; }

	}
}
