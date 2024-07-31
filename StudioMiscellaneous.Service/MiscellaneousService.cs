using StackExchange.Redis;
using StudioMiscellaneous.Service.Contract;
using StudioRoomType.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StudioMiscellaneous.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class MiscellaneousService : IStudioMiscellaneousService
    {
        Dictionary<int,Room> _roomType = new Dictionary<int, Room>();

        Func<Room, int> _createRoomType;
        Func<Room, int> _updateRoomType;
        Func<int, int> _deleteRoomType;


        public MiscellaneousService(
            Func<IEnumerable<Room>> getAllRoomType,
            Func<Room,int> createRoomType,
            Func<Room,int> updateRoomType,
            Func<int,int> deleteRoomType

            ) 
        {

            _createRoomType = createRoomType;
            _updateRoomType = updateRoomType;
            _deleteRoomType = deleteRoomType;

            foreach(var room in getAllRoomType())
            {
                _roomType.Add(room.Id, room);
            }

        }

        //todo
        public int CreateRoomType(Room Room)
        {

            Room.Style = string.Join(",", Room.StyleArr);

            var id = _createRoomType(Room);

            if(id > 0)
            {
                Room.Id = id;

                _roomType[id] = Room;

                // todo : call redis publish

                return 0;
            }

            return -10;

        }

        public int DeleteRoomType(int RoomId)
        {

            var error = _deleteRoomType(RoomId);

            if(error == 0)
            {
                _roomType.Remove(RoomId);

                // todo : call redis publish

                return 0;
            }

            return -10;

        }

        public void Dispose()
        {
        }

        public int EditRoomType(int RoomId,string Description,string Size, string Image, string[] Style, decimal Rate,DateTime UpdateTime,string UpdateBy)
        {
            
             Room roomInfo;
             if(_roomType.TryGetValue(RoomId,out roomInfo))
             {
                var r = new Room()
                {
                    Id = roomInfo.Id,
                    Name = roomInfo.Name,
                    Description = Description,
                    Image = Image,
                    Size = Size,
                    Style = string.Join(",",Style),
                    Rate = Rate,
                    CreateBy = roomInfo.CreateBy,
                    CreatedDate = roomInfo.CreatedDate,
                    UpdateBy = UpdateBy,
                    UpdatedDate = UpdateTime
                };
                   
                var error = _updateRoomType(roomInfo);

                if(error == 0)
                {
                    _roomType[r.Id] = r;

                    // todo : call redis publish

                    return 0;
                }


                return -10;
             }

            return -11;
        }

        public RoomsViewModel GetAllRoomType()
        {

            var rooms = new List<Room>();

            foreach(var room in _roomType.Values)
            {
                rooms.Add(room);
            }

            return new RoomsViewModel
            {
                Error = 0,
                Rooms = rooms
            };
        }
    }
}
