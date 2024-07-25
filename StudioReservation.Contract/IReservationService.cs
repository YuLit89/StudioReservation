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
        int CreateTimeSlot(CreateRoomTimeSlot Request);
        [OperationContract]
        int EditTimeSlot(long TimeSlotId, string Times, string UpdateBy, DateTime UpdateTime, bool Enable);
        [OperationContract]
        ViewAllTimeSlot FindAllRoomTimeSlot(int Page, long LastId, int Size = 20);
        [OperationContract]
        ViewAllTimeSlot FindRoomTimeSlotByFilter(int RoomId, DateTime StartTime, DateTime EndTime, int Page, long LastId, int Size = 20);

        [OperationContract]
        RoomTimeSlotDetail FindDetail(long TimeSlotId);

        [OperationContract]
        int TimeSlotReservation(TimeSlotReservationRequest Request);

    }
}
