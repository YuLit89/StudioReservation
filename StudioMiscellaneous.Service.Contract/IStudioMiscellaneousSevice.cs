using StudioFeedBack.DataModel;
using StudioRoomType.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudioMiscellaneous.Service.Contract
{
    [ServiceContract]
    public interface IStudioMiscellaneousService : IDisposable
    {
        [OperationContract]
        int CreateRoomType(Room Room);

        [OperationContract]
        RoomsViewModel GetAllRoomType();

        [OperationContract]
        int EditRoomType(Room room);

        [OperationContract]
        int DeleteRoomType(int RoomId);

        [OperationContract]
        RoomViewDetail FindRoomDetail(int RoomId);

        [OperationContract]
        FeedbackViewModel GetAllByStatus(int status);

        [OperationContract]
        SubmitFeedbackResponse SubmitFeedback(SubmitFeedback Feedback, DateTime SubmitTime);

        [OperationContract]
        int ReplyFeedback(string AdminId, string TicketId, string Message, DateTime SubmitTime, bool isComplete);

        [OperationContract]
        FeedbackViewModel ViewDetail(long Id);


    }
}
