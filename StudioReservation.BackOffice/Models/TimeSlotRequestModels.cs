using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudioReservation.BackOffice.Models
{
    public class CreateTimeSlot
    {
        public int RoomId { get; set; }
        public string Dates { get; set; }
        public string Times { get; set; }
        public bool Enable { get; set; }
    }

    public class EditTimeSlot
    {
        public long Id { get; set; }
        public string Times { get; set; }
        public bool Enable { get; set; }
    }
}