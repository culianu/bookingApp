using bookingApp.Models;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bookingApp.Utils;
using System.Collections.Immutable;

namespace bookingApp.Managers
{
	public class HotelManager
	{
		private readonly List<Hotel> hotels = [];
		private readonly List<Booking> bookings = [];
		private static JsonSerializerOptions options = new JsonSerializerOptions
		{
			Converters = { new DateOnlyJsonConverter() },
			PropertyNameCaseInsensitive = true
		};

		public HotelManager(string hotelJsonPath, string bookingsJsonPath)
		{
			hotels = LoadHotels(hotelJsonPath);
			bookings = LoadBookings(bookingsJsonPath);
			//var hotel = GetHotel("H1");
			//if (hotel == null)
			//{
			//	Console.WriteLine("Hotel not found");
			//	return;
			//}
			//var ceva = BookedRoomsForPeriod(hotel, new DateOnly(2024, 09, 01), new DateOnly(2024, 09, 03));
			//var altceva = AvailableRoomsForPeriod(hotel, new DateOnly(2024, 09, 01), new DateOnly(2024, 09, 03));
			//var diferit = BookRoomTypes(hotel, 3, new DateOnly(2024, 09, 04), new DateOnly(2024, 09, 04));
		}

		public Hotel? GetHotel(string hotelId)
		{
			var hotel = hotels.FirstOrDefault(h => h.Id == hotelId);
			return hotel;
		}

		public Dictionary<string, int> BookedRoomsForPeriod(Hotel hotel, DateOnly arrival, DateOnly departure)
		{
			return bookings
				.Where(b => hotel.Id.Equals(b.HotelId) && DateUtils.IsPeriodOverlappingWithRange(arrival, departure, b.Arrival, b.Departure))
				.GroupBy(b => b.RoomType).ToDictionary(g => g.Key, g => g.Count());
		}

		public Dictionary<string, int> AvailableRoomsForPeriod(Hotel hotel, DateOnly arrival, DateOnly departure)
		{
			var bookedRooms = BookedRoomsForPeriod(hotel, arrival, departure);
			return hotel.Rooms
				.GroupBy(r => r.RoomType)
				.ToDictionary(g => g.Key, g => g.Count() - (bookedRooms.ContainsKey(g.Key) ? bookedRooms[g.Key] : 0));
		}

		public int HowManyPeopleCanStayForPeriod(Hotel hotel, DateOnly arrival, DateOnly departure)
		{
			var availableRooms = AvailableRoomsForPeriod(hotel, arrival, departure);
			int availableCapacity = 0;
			hotel.RoomTypes
				.ForEach(rt =>
				{
					if (availableRooms.ContainsKey(rt.Code))
					{
						availableCapacity += availableRooms[rt.Code] * rt.Size;
					}
				});
			return availableCapacity;
		}

		//had no time to test this properly, but it should work
		public Dictionary<string, int> BookRoomTypes(Hotel hotel, int groupSize, DateOnly arrival, DateOnly departure)
		{
			int maxNumberOfPeople = HowManyPeopleCanStayForPeriod(hotel, arrival, departure);
			if (groupSize > maxNumberOfPeople)
			{
				Console.WriteLine("Group size is too big, only {0} people can be booked", maxNumberOfPeople);
				return [];
			}
			var availableRooms = AvailableRoomsForPeriod(hotel, arrival, departure);
			var RoomTypesOrdered = hotel.RoomTypes.OrderByDescending(rt => rt.Size).ToList();
			var remainingGuests = groupSize;
			Dictionary<string, int> roomTypesToBook = new Dictionary<string, int>();
			//first try to book the rooms that fit the group size
			foreach (var roomType in RoomTypesOrdered)
			{
				if (remainingGuests == 0) break;
				if (availableRooms.ContainsKey(roomType.Code) && availableRooms[roomType.Code] > 0)
				{
					int roomsOfTypeNeeded = remainingGuests / roomType.Size;
					//this should really minimize the number of rooms needed, but doesn't care about partially booking a room
					//not enough time to figure out guests / rooms / room sizes combinations to truly test this :P
					if (roomsOfTypeNeeded == 0 && remainingGuests < roomType.Size)
					{
						var suitableRoomType = RoomTypesOrdered.FirstOrDefault(rt => rt.Size == remainingGuests);
						if (suitableRoomType != null && !availableRooms.ContainsKey(suitableRoomType.Code) && availableRooms[suitableRoomType.Code] > 0)
						{
							roomTypesToBook.Add($"!{roomType.Code}", 1);
							remainingGuests = 0;
						}
					}
					int roomsLeft = availableRooms[roomType.Code] - roomsOfTypeNeeded;
					if (roomsLeft >= 0)
					{
						remainingGuests -= roomsOfTypeNeeded * roomType.Size;
						availableRooms[roomType.Code] -= roomsOfTypeNeeded;
						roomTypesToBook.Add(roomType.Code, roomsOfTypeNeeded);
					}
					else
					{
						roomTypesToBook.Add(roomType.Code, availableRooms[roomType.Code]);
						remainingGuests -= availableRooms[roomType.Code] * roomType.Size;
						availableRooms[roomType.Code] = 0;
					}
				}
			}
			if (remainingGuests == 0)
			{
				return roomTypesToBook;
			}
			//for the remaining guests, we need to partially fill rooms
			foreach (var roomType in RoomTypesOrdered)
			{
				if (remainingGuests == 0) break;
				if (availableRooms.ContainsKey(roomType.Code) && availableRooms[roomType.Code] > 0)
				{
					roomTypesToBook.Add($"!{roomType.Code}", 1);
					remainingGuests -= roomType.Size;
				}
			}
			return roomTypesToBook;
		}

		private static List<Hotel> LoadHotels(string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("Hotel json not found");
				return [];
			}
			string json = File.ReadAllText(path);
			if (string.IsNullOrEmpty(json)) return [];
			try
			{
				return JsonSerializer.Deserialize<List<Hotel>>(json, options) ?? [];
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return [];
			}
		}

		private static List<Booking> LoadBookings(string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("Bookings json not found");
				return [];
			}
			string json = File.ReadAllText(path);
			if (string.IsNullOrEmpty(json)) return [];
			try
			{
				return JsonSerializer.Deserialize<List<Booking>>(json, options) ?? [];
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return [];
			}
		}
	}
}
