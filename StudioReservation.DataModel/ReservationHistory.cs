using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioReservation.DataModel
{
    public class ReservationHistory
    {
        public long Id { get; set; }
        public int RoomId { get; set; }
        public DateTime DateTime { get; set; }
        public int Status { get; set; }
        public string ReservationBy { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string BookingId { get; set; }
        public string Remark { get; set; }
        public decimal Price { get; set; }
    }
}
