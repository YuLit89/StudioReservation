using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudioReservation.BackOffice.Models
{
    public class CreateTimeSlotRequest
    {
        public int RoomId { get; set; }
        public string Dates { get; set; }
        public string[] Times { get; set; }
        public bool Enable { get; set; }
    }

    public class EditTimeSlot
    {
        public long Id { get; set; }
        public string RoomName { get; set; }
        public bool Enable { get; set; }
        public string Date { get; set; }
        public string[] Times { get; set; }
        public string DisabledTimes { get; set; }
        [NotMapped]
        public Dictionary<string, int> TimeDic{ get; set; }
    }
}