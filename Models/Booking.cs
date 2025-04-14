using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookingApp.Models
{
	public class Booking
	{
		public required string HotelId { get; set; }
		public DateOnly Arrival { get; set; }
		public DateOnly Departure { get; set; }
		public required string RoomType { get; set; }
		public string RoomRate { get; set; } = string.Empty;
	}
}
