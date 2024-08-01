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

    }
}
