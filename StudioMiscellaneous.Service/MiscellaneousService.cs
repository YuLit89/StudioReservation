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
using Redis;
using StudioFeedBack.DataModel;

namespace StudioMiscellaneous.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class MiscellaneousService : IStudioMiscellaneousService
    {
        Dictionary<int,Room> _roomType = new Dictionary<int, Room>();

        Func<Room, int> _createRoomType;
        Func<Room, int> _updateRoomType;
        Func<int, int> _deleteRoomType;

        Func<int,FeedbackViewModel> _getAllUserFeedback;
        Func<SubmitFeedback, DateTime, SubmitFeedbackResponse> _submitFeedback;
        Func<string, string, string, DateTime, bool, int> _replyFeedback;
        Func<long, FeedbackViewModel> _findFeedbackDetail;

        IRedisConnection _redis;

        const string _syncRoomChannel = "sync-roomType";

        public MiscellaneousService(
            IRedisConnection redisConnection,
            Func<IEnumerable<Room>> getAllRoomType,
            Func<Room,int> createRoomType,
            Func<Room,int> updateRoomType,
            Func<int,int> deleteRoomType,
            Func<int,FeedbackViewModel> getAllUserFeedback,
            Func<SubmitFeedback,DateTime,SubmitFeedbackResponse> submitFeedback,
            Func<string,string,string,DateTime,bool,int> replyFeedback,
            Func<long,FeedbackViewModel> findFeedbackDetail

            ) 
        {
            _redis = redisConnection;

            _createRoomType = createRoomType;
            _updateRoomType = updateRoomType;
            _deleteRoomType = deleteRoomType;


            _getAllUserFeedback = getAllUserFeedback;
            _submitFeedback = submitFeedback;
            _replyFeedback = replyFeedback;
            _findFeedbackDetail = findFeedbackDetail;


            foreach(var room in getAllRoomType())
            {
                _roomType.Add(room.Id, room);

            }

            Console.WriteLine($"{DateTime.Now} || done pump room type {_roomType.Count()}");
        }

        //todo
        public int CreateRoomType(Room Room)
        {
            var id = _createRoomType(Room);

            if(id > 0)
            {
                Room.Id = id;

                _roomType[id] = Room;

                _redis.Publish<Room>(_syncRoomChannel, Room);

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

                _redis.Publish<bool>("delete-roomType", true);

                return 0;
            }

            return -10;

        }

        public void Dispose()
        {
        }

        public int EditRoomType(Room room)
        {
            
             Room roomInfo;
             if(_roomType.TryGetValue(room.Id,out roomInfo))
             {
                var r = new Room()
                {
                    Id = roomInfo.Id,
                    Name = room.Name,
                    Description = room.Description,
                    Image = room.Image,
                    Size = room.Size,
                    Style = room.Style,
                    Rate = room.Rate,
                    CreateBy = roomInfo.CreateBy,
                    CreatedDate = roomInfo.CreatedDate,
                    UpdateBy = room.UpdateBy,
                    UpdatedDate = room.UpdatedDate
                };
                   
                var error = _updateRoomType(r);

                if(error == 0)
                {
                    _roomType[r.Id] = r;

                    _redis.Publish<Room>(_syncRoomChannel, r);
                    
                    return 0;
                }


                return -10;
             }

            return -11;
        }

        public RoomViewDetail FindRoomDetail(int RoomId)
        {
            Room room;

            return _roomType.TryGetValue(RoomId, out room)
                   ? new RoomViewDetail { Room = room, Error = 0 }
                   : new RoomViewDetail { Room = new Room(), Error = -10 };
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

        public FeedbackViewModel GetAllByStatus(int status)
        {
            return _getAllUserFeedback(status);
        }

        public SubmitFeedbackResponse SubmitFeedback(SubmitFeedback Feedback, DateTime SubmitTime)
        {
            return _submitFeedback(Feedback, SubmitTime);
        }

        public int ReplyFeedback(string AdminId, string TicketId, string Message, DateTime SubmitTime, bool isComplete)
        {
            return _replyFeedback(AdminId, TicketId, Message, SubmitTime, isComplete);
        }

        public FeedbackViewModel ViewDetail(long Id)
        {
            return _findFeedbackDetail(Id);
        }
    }
}
