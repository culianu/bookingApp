using BookingApp.Managers;
using BookingApp.Models;
using myAppTest.MockData;
using Xunit;

namespace myAppTest
{
	public class HotelManagerTests
	{
		private readonly HotelManager _hotelManager;

		public HotelManagerTests()
		{
			var hotels = MockHotels.GetHotels();
			var bookings = MockBookings.GetBookings();
			_hotelManager = new HotelManager(hotels, bookings);
		}

		[Fact]
		public void GetHotel_ShouldReturnHotel_WhenHotelExists()
		{
			var result = _hotelManager.GetHotel("H1");

			Assert.NotNull(result);
			Assert.Equal("H1", result?.Id);
			Assert.Equal("Hotel California", result?.Name);
		}

		[Fact]
		public void GetHotel_ShouldReturnNull_WhenHotelDoesNotExist()
		{
			var result = _hotelManager.GetHotel("NonExistentHotel");

			Assert.Null(result);
		}

		[Fact]
		public void BookedRoomsForPeriod_ShouldReturnCorrectCounts()
		{
			var hotel = _hotelManager.GetHotel("H1");

			var result = _hotelManager.BookedRoomsForPeriod(hotel!, new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3));

			Assert.NotNull(result);
			Assert.True(result.ContainsKey("DBL"));
			Assert.Equal(1, result["DBL"]);
			Assert.True(result.ContainsKey("SGL"));
			Assert.Equal(1, result["SGL"]);
		}

		[Fact]
		public void AvailableRoomsForPeriod_ShouldReturnCorrectCounts()
		{
			var hotel = _hotelManager.GetHotel("H1");

			var result = _hotelManager.AvailableRoomsForPeriod(hotel!, new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3));

			Assert.NotNull(result);
			Assert.True(result.ContainsKey("SGL"));
			Assert.Equal(1, result["SGL"]);
			Assert.True(result.ContainsKey("DBL"));
			Assert.Equal(1, result["DBL"]);
		}

		[Fact]
		public void HowManyPeopleCanStayForPeriod_ShouldReturnCorrectCapacity()
		{
			var hotel = _hotelManager.GetHotel("H1");

			var result = _hotelManager.HowManyPeopleCanStayForPeriod(hotel!, new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3));

			Assert.Equal(3, result); // 1 SGL and 1 DBL
		}

		[Fact]
		public void BookRoomTypes_ShouldReturnCorrectRoomTypesToBook()
		{
			var hotel = _hotelManager.GetHotel("H1");

			var result = _hotelManager.BookRoomTypes(hotel!, 3, new DateOnly(2024, 9, 3), new DateOnly(2024, 9, 4));

			Assert.NotNull(result);
			Assert.True(result.ContainsKey("DBL"));
			Assert.Equal(1, result["DBL"]);
			Assert.True(result.ContainsKey("SGL"));
			Assert.Equal(1, result["SGL"]);
		}
	}
}
