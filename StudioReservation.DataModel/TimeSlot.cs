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
        public bool Enable { get; set; }
    }

    
    public class RoomTimeSlot : RoomTimeSlotBase
    {
        public DateTime Date { get; set; } // only 1 date
        public string Times { get; set; } //can be multi times 
        public string CreatedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class CreateRoomTimeSlot : RoomTimeSlotBase
    {
        public string Dates { get; set; }
        public string Times { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string UpdateBy { get; set; }
 
    }

    public class ViewAllTimeSlot 
    {
        public List<ViewTimeSlot> TimeSlots { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public int TotalPage { get; set; }
        public int Paging { get; set; }
        public long LastId { get; set; }
        public int Error { get; set; }
    }

    public class ViewTimeSlot : RoomTimeSlotBase
    {
        public DateTime Date { get; set; }
        public bool AbleToDelete { get; set; }
        public List<string> AvailableTime { get; set; }
        public List<string> BookedTime { get; set; }
    }

    public class RoomTimeSlotDetail : RoomTimeSlotBase
    {
        public DateTime Date { get; set; }
        public DateTime CreatedTime { get; set; }

        public List<string> AvailableTime { get; set; }
        public List<string> BookedTime { get; set; }
        public List<string> LockedTime { get; set; }

        public string RoomTypeImage { get; set; }

        public bool EnableEdit { get; set; }

        public int Error { get; set; }

    }

    public class TimeSlotReservationRequest
    {
        public string MemberId { get; set; }
        public int RoomId { get; set; }
        public string ReservationTime { get; set; }
    }

    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
