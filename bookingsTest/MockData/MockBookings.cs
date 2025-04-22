using BookingApp.Models;

namespace myAppTest.MockData
{
	public static class MockBookings
	{
		public static List<Booking> GetBookings()
		{
			return new List<Booking>
			{
				new Booking
				{
					HotelId = "H1",
					Arrival = new DateOnly(2024, 9, 1),
					Departure = new DateOnly(2024, 9, 3),
					RoomType = "DBL",
					RoomRate = "Prepaid"
				},
				new Booking
				{
					HotelId = "H1",
					Arrival = new DateOnly(2024, 9, 2),
					Departure = new DateOnly(2024, 9, 5),
					RoomType = "SGL",
					RoomRate = "Standard"
				}
			};
		}
	}
}