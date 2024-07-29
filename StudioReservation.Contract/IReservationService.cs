using StudioReservation.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudioReservation.Contract
{
    [ServiceContract]
    public interface IReservationService : IDisposable
    {

        [OperationContract]
        NotAvailableRoomDate GetNotAvailableRoomDate(int RoomId);

        //[OperationContract]
        //GetNotAvailableDateTime(int RoomId, int TimeRange);

        [OperationContract]
        int CreateTimeSlot(CreateRoomTimeSlot Request);
        [OperationContract]
        int EditTimeSlot(long TimeSlotId, string Times, string UpdateBy, DateTime UpdateTime, bool Enable);
        [OperationContract]
        ViewAllTimeSlot FindAllRoomTimeSlot();
        //[OperationContract]
        //ViewAllTimeSlot FindRoomTimeSlotByFilter(int RoomId, DateTime StartTime, DateTime EndTime);

        [OperationContract]
        RoomTimeSlotDetail FindDetail(long TimeSlotId);

        [OperationContract]
        int UpdateSuccessReservation(long ReservationId);

        [OperationContract]
        int LockTimeSlotReservation(TimeSlotReservationRequest Request);


        [OperationContract]
        int DeleteTimeSlot(long TimeSlotId);

        //[OperationContract]
        //ScheduleViewModel ReservationSchedule(long RoomId, string selectedDate);


    }
}
