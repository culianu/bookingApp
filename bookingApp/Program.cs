using BookingApp.Managers;
using BookingApp.Models;

public class HotelBookingApp
{
	private const string DateFormat = "yyyyMMdd";
	public static void Main(string[] args)
	{
		if (args.Length != 4 || args[0] != "--hotels" || args[2] != "--bookings")
		{
			Console.WriteLine("Usage: bookingApp --hotels <hotels.json> --bookings <bookings.json>");
			return;
		}

		string hotelsFilePath = args[1];
		string bookingsFilePath = args[3];

		if (!File.Exists(hotelsFilePath) || !File.Exists(bookingsFilePath))
		{
			Console.WriteLine("Error: One or both of the specified files do not exist.");
			return;
		}

		const string invalidCommandMessage = $"Invalid command. Example:  \nAvailability(H1, 20240901, SGL)" +
					$"\nAvailability(H1, 20240901-20240903, DBL) " +
					$"\nRoomTypes(H1, 20240904, 3) " +
					$"\nRoomTypes(H1, 20240905-20240907, 5) ";

		HotelManager hotelManager = new HotelManager(hotelsFilePath, bookingsFilePath);
		string? input;
		Console.WriteLine("Enter commands (Availability or RoomTypes) or a blank line to exit:");

		while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
		{
			try
			{
				if (input.StartsWith("Availability"))
				{
					var parts = input.Substring("Availability(".Length, input.Length - "Availability(".Length - 1).Split(',');
					if (parts.Length == 3)
					{
						string hotelId = parts[0].Trim();
						string dateRange = parts[1].Trim();
						string roomType = parts[2].Trim();

						ParseHotelIdAndDateRange(hotelId, dateRange, hotelManager, out var hotel, out var arrival, out var departure);
						var availableRooms = hotelManager.AvailableRoomsForPeriod(hotel, arrival, departure);
						var numberOfRoomsAvailable = availableRooms.ContainsKey(roomType) ? availableRooms[roomType] : 0;
						Console.WriteLine($"Room Type: {roomType}, Available Rooms: {numberOfRoomsAvailable}\n");
					}
					else
					{
						Console.WriteLine("Invalid Availability command format. Example: Availability(H1, 20240901, SGL) or Availability(H1, 20240901-20240903, DBL)");
					}
				}
				else if (input.StartsWith("RoomTypes"))
				{
					var parts = input.Substring("RoomTypes(".Length, input.Length - "RoomTypes(".Length - 1).Split(',');
					if (parts.Length == 3)
					{
						string hotelId = parts[0].Trim();
						string dateRange = parts[1].Trim();
						int numberOfPeople = int.Parse(parts[2].Trim());

						ParseHotelIdAndDateRange(hotelId, dateRange, hotelManager, out var hotel, out var arrival, out var departure);
						var roomTypes = hotelManager.BookRoomTypes(hotel, numberOfPeople, arrival, departure);
						roomTypes.Keys.ToList().ForEach(k => RepeatWriteOnConsole(k, roomTypes[k]));
						Console.WriteLine('\n');
					}
					else
					{
						Console.WriteLine("Invalid RoomTypes command format. Example: RoomTypes(H1, 20240904, 3) or RoomTypes(H1, 20240905-20240907, 5)");
					}
				}
				else
				{
					Console.WriteLine(invalidCommandMessage);
				}
			}
			catch (FormatException)
			{
				Console.WriteLine("Invalid date or number format.");
			}
			catch
			{
				Console.WriteLine(invalidCommandMessage);
			}
		}
	}

	private static void RepeatWriteOnConsole(string message, int nrOfTimes)
	{
		for (var i = 0; i < nrOfTimes; i++) Console.Write($"{message} ");
	}

	private static void ParseHotelIdAndDateRange(string hotelId, string dateRange, HotelManager hotelManager, out Hotel hotel, out DateOnly arrival, out DateOnly departure)
	{
		var retrievedHotel = hotelManager.GetHotel(hotelId);
		if (retrievedHotel == null)
		{
			throw new ArgumentException($"Hotel with ID '{hotelId}' not found.");
		}
		hotel = retrievedHotel;
		if (dateRange.Contains('-'))
		{
			var dates = dateRange.Split('-');
			arrival = DateOnly.ParseExact(dates[0].Trim(), DateFormat);
			departure = DateOnly.ParseExact(dates[1].Trim(), DateFormat);
		}
		else
		{
			arrival = DateOnly.ParseExact(dateRange, DateFormat);
			departure = arrival;
		}
	}
}