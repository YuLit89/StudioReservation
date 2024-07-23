using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioReservation.DataModel
{

    public class RoomTimeSlotBase
    {
        public long Id { get; set; }
        public int RoomId { get; set; }
        public string Times { get; set; }
        public bool Enable { get; set; }
    }

    public class RoomTimeSlot : RoomTimeSlotBase
    {
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }

    }

    public class RoomTimeSlotRequest : RoomTimeSlotBase
    {
        public string Dates { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class ViewAllTimeSlot 
    {
        public List<ViewTimeSlot> TimeSlots { get; set; }
        public int TotalPage { get; set; }
        public int Paging { get; set; }
        public long LastId { get; set; }
        public int Error { get; set; }
    }

    public class ViewTimeSlot : RoomTimeSlotBase
    {
        public DateTime Date { get; set; }
        public bool AbleToDelete { get; set; }
    }


    public class ViewTimeSlotDetail
    {
        public long Id { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<DateTime, int> Times { get; set; }
        public bool Enable { get; set; }

    }

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
        public decimal Price {  get; set; }
    }

    public class TimeSlotReservationRequest
    {
        public string MemberId { get; set; }
        public int RoomId { get; set; }
        public string ReservationTime { get; set; }
    }
}
