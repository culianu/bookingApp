using BookingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myAppTest.MockData
{
	public static class MockHotels
	{
		public static List<Hotel> GetHotels()
		{
			return new List<Hotel>
			{
				new Hotel
				{
					Id = "H1",
					Name = "Hotel California",
					RoomTypes = new List<RoomType>
					{
						new RoomType
						{
							Code = "SGL",
							Size = 1,
							Description = "Single Room",
							Amenities = new List<string> { "WiFi", "TV" },
							Features = new List<string> { "Non-smoking" }
						},
						new RoomType
						{
							Code = "DBL",
							Size = 2,
							Description = "Double Room",
							Amenities = new List<string> { "WiFi", "TV", "Minibar" },
							Features = new List<string> { "Non-smoking", "Sea View" }
						}
					},
					Rooms = new List<Room>
					{
						new Room { RoomType = "SGL", RoomId = "101" },
						new Room { RoomType = "SGL", RoomId = "102" },
						new Room { RoomType = "DBL", RoomId = "201" },
						new Room { RoomType = "DBL", RoomId = "202" }
					}
				}
			};
		}
	}
}
