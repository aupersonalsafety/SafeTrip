using System;
namespace SafeTrip
{
	public class GlobalPosition
	{
		public GlobalPosition()
		{
		}

		public GlobalPosition(double lat, double longitude)
		{
			Latitude = lat;
			Longitude = longitude;
		}

		public double Latitude { get; set; }
		public double Longitude { get; set; }

	}
}
