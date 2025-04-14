using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookingApp.Models
{
	public class Hotel
	{
		public required string Id { get; set; }
		public required string Name { get; set; }
		public List<RoomType> RoomTypes { get; set; } = new List<RoomType>();
		public List<Room> Rooms { get; set; } = new List<Room>();
	}
}
