
using StudioReservation.Contract;
using StudioReservation.DataModel;
using StudioRoomType.DataModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StudioReservation.Service
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class ReservationService : IReservationService
    {

        Dictionary<long,RoomTimeSlot> _roomTimeSlot = new Dictionary<long,RoomTimeSlot>();

        Dictionary<int, Room> _roomType = new Dictionary<int, Room>();

        Dictionary<long,ReservationHistory> _reservation = new Dictionary<long,ReservationHistory>();
        Dictionary<DateTime,int> _dateBooked = new Dictionary<DateTime, int>();


        Func<RoomTimeSlot, long> _insertTimeSlot;
        Func<long, string, string, DateTime, bool, long> _updateTimeSlot;
        Func<ReservationHistory, long> _insertReservation;
        
        ConcurrentDictionary<int, object> _sync = new ConcurrentDictionary<int, object>();

        readonly int _timeSlotRange;

        public ReservationService(
            Func<IEnumerable<RoomTimeSlot>> getAllTimeSlot,
            Func<RoomTimeSlot, long> insertTimeSlot,
            Func<long,string,string,DateTime,bool,long> updateTimeSlot,
            Func<ReservationHistory,long> insertReservation,
            Func<IEnumerable<Room>> getAllRoomsType,
            int timeSlotRange)
        {
            _insertTimeSlot = insertTimeSlot;
            _updateTimeSlot = updateTimeSlot;

            _insertReservation = insertReservation;

            _timeSlotRange = timeSlotRange; 


            Init(getAllTimeSlot(), getAllRoomsType());

        }

        internal void Init(IEnumerable<RoomTimeSlot> getAllTimeSlot ,IEnumerable<Room> getRoomType)
        {
            foreach(var timeSlot in getAllTimeSlot)
            {
                _roomTimeSlot.Add(timeSlot.Id, timeSlot);
            }

            foreach(var room in getRoomType)
            {
                _roomType.Add(room.Id, room);
            }
        }

        public int CreateTimeSlot(CreateRoomTimeSlot request)
        {

            var timeSlots = new List<RoomTimeSlot>();

            var dates = request.Dates.Split(',');

            using (var process = new TransactionScope())
            {
                foreach ( var d in dates)
                {
                    var date = Convert.ToDateTime(d);

                    var timeSlot = new RoomTimeSlot
                    {
                        Date = date,
                        RoomId = request.RoomId,
                        Times = request.Times,
                        CreatedBy = request.CreatedBy,
                        CreateTime = request.CreateTime,
                        UpdateBy = request.UpdateBy,
                        UpdateTime = request.UpdateTime,
                        Enable = request.Enable
                    };

                    var id = _insertTimeSlot(timeSlot);

                    if(id <= 0)
                    {
                        process.Dispose();

                        return -10;
                    }

                    timeSlot.Id = id;
                    timeSlots.Add(timeSlot);
                }

                process.Complete();
            }

            foreach(var slot in timeSlots)
            {
                _roomTimeSlot[slot.Id] = slot;
            }

            return 0;
        }

        public int EditTimeSlot(long TimeSlotId, string Times, string UpdateBy, DateTime UpdateTime, bool Enable)
        {

            var error = _updateTimeSlot(TimeSlotId, Times, UpdateBy, UpdateTime, Enable);

            if(error < 0)
            {
                return -10;
            }

            if(_roomTimeSlot.TryGetValue(TimeSlotId, out RoomTimeSlot timeSlot))
            {
                timeSlot.Times = Times;
                timeSlot.UpdateTime = UpdateTime;
                timeSlot.UpdateBy = UpdateBy;
                timeSlot.Enable = Enable;

            }

            return 0;
        }

        public ViewAllTimeSlot FindAllRoomTimeSlot(int Page,long LastId ,int Size = 20)
        {
            var now = DateTime.Now;

            int totelPage = _roomTimeSlot.Values.Where(x => x.Date >= now.Date).Count()/Size;

            var sizeTaken = (LastId == 0) ? Size : Size * (Page - 1);

            var timeSlots = (LastId == 0)
                           ? _roomTimeSlot.Values.Where(x => x.Date >= now.Date).OrderBy(x => x.Id).Take(sizeTaken).ToList()
                           : _roomTimeSlot.Values.Where(x => x.Id > LastId).OrderBy(x => x.Id).Take(sizeTaken).Skip(sizeTaken-Size);

            var history = InternalComputeTimeSlotHistory(timeSlots.ToList());

            return new ViewAllTimeSlot
            {
                TimeSlots = history,
                Error = 0,
                TotalPage = (totelPage == 0 ) ? 1 : totelPage,
                Paging = (Page == 0) ? 1 : Page,
                LastId = (history.Count() > 0) ? history.Last().Id : 0,
            };
        }

        public ViewAllTimeSlot FindRoomTimeSlotByFilter(int RoomId, DateTime StartDate, DateTime EndDate, int Page, long LastId, int Size = 20)
        {
            var now = DateTime.Now;

            IEnumerable<RoomTimeSlot> query;

            if(RoomId != 0)
            {
                query = _roomTimeSlot.Where(x => x.Value.RoomId == RoomId).Select(x => x.Value);
            }

            int totelPage = _roomTimeSlot.Values.Where(x => x.Date >= now.Date).Count() / Size;

            var sizeTaken = (LastId == 0) ? Size : Size * (Page - 1);

            var timeSlots = (LastId == 0)
                           ? _roomTimeSlot.Values.Where(x => x.Date >= StartDate && x.Date <= EndDate).OrderBy(x => x.Id).Take(sizeTaken)
                           : _roomTimeSlot.Values.Where(x => x.Id > LastId).OrderBy(x => x.Id).Take(sizeTaken).Skip(sizeTaken-Size);

            var history = InternalComputeTimeSlotHistory(timeSlots.ToList());

            return new ViewAllTimeSlot
            {
                TimeSlots = history,
                Error = 0,
                TotalPage = (totelPage == 0) ? 1 : totelPage,
                Paging = (Page == 0) ? 1 : Page,
                LastId = (history.Count() > 0) ? history.Last().Id : 0
            };
        }

        public RoomTimeSlotDetail FindDetail(long TimeSlotId)
        {
            var now = DateTime.Now;

            RoomTimeSlot info;
            if (!_roomTimeSlot.TryGetValue(TimeSlotId, out info))
            {
                return new RoomTimeSlotDetail
                {
                    Error = -10
                };
            }

            Room room;
            if (!_roomType.TryGetValue(info.RoomId, out room))
            {
                return new RoomTimeSlotDetail
                {
                    Error = -11
                };
            }

            Dictionary<string, int> timeStatus = new Dictionary<string, int>();

            foreach (var time in info.Times.Split(','))
            {
                var day = TimeSpan.Parse(time);

                var dateTime = new DateTime(info.Date.Year, info.Date.Month, info.Date.Day, day.Hours, day.Minutes, day.Seconds);

                int status;
                if (!_dateBooked.TryGetValue(dateTime, out status))
                {
                    timeStatus.Add(time, (int)ReservationStatus.Opening);
                }
                else
                {
                    if (status == (int)ReservationStatus.Booked) timeStatus.Add(time, (int)ReservationStatus.Booked);
                    if (status == (int)ReservationStatus.Lock) timeStatus.Add(time, (int)ReservationStatus.Lock);
                }
            }

            return new RoomTimeSlotDetail()
            {
                Id = info.Id,
                RoomId = info.RoomId,
                RoomName = room.Name,
                Enable = info.Enable,
                Date = info.Date.ToString("yyyy-MM-dd"),
                CreatedTime = info.CreateTime,
                AvailableTime = timeStatus.Where(x => x.Value == (int)ReservationStatus.Opening).Select(x => x.Key).ToList(),
                BookedTime = timeStatus.Where(x => x.Value == (int)ReservationStatus.Booked).Select(x => x.Key).ToList(),
                LockedTime = timeStatus.Where(x => x.Value == (int)ReservationStatus.Lock).Select(x => x.Key).ToList(),
                RoomTypeImage = room.Image,
                EnableEdit = (now.Date > info.Date.Date) ? false : true,
                Error = 0
            };
        }

        public NotAvailableRoomDate GetNotAvailableRoomDate(int RoomId)
        {

            var now = DateTime.Now;
            var limitDate = now.AddDays(_timeSlotRange);

            var room = new List<RoomType>();

            foreach (var r in _roomType.Values)
            {
                var r1 = new RoomType { Id = r.Id, Name = r.Name };

                room.Add(r1);
            }

            var i = _roomTimeSlot.Values.Where(x => x.RoomId == RoomId).Where(x => x.Date >= now && x.Date <= limitDate).Select(x => x.Date.ToString("yyyy-MM-dd")).Distinct().ToList();

            return new NotAvailableRoomDate
            {
                Room = room,
                NotAvailableDates = string.Join(",", i),
                Error = 0,
            };
        }

        public int TimeSlotReservation(TimeSlotReservationRequest Request)
        {

            var times = Request.ReservationTime.Split(',');

            var thread = _sync.GetOrAdd(Request.RoomId, new object());

            var reservations = new List<ReservationHistory>();

            lock (thread)
            {
                var uuid = Guid.NewGuid().ToString();

                foreach (var t in times)
                {
                    var d = Convert.ToDateTime(t);

                    int status;
                    if (_dateBooked.TryGetValue(d, out status))
                    {
                        if (status == (int)ReservationStatus.Lock) return -100;
                        if (status == (int)ReservationStatus.Booked) return -101;
                        if (status == (int)ReservationStatus.Closed) return -102;
                    }


                    var r = new ReservationHistory
                    {
                        RoomId = Request.RoomId,
                        DateTime = d,
                        Status = (int)ReservationStatus.Booked,
                        ReservationBy = Request.MemberId,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        BookingId = uuid,
                        Remark = string.Empty,
                        Price = 0
                    };

                    reservations.Add(r);
                }

                using (var process = new TransactionScope())
                {
                    // insert to new table - member history

                    foreach (var r in reservations)
                    {
                        var id = _insertReservation(r);
                        if (id <= 0)
                        {
                            process.Dispose();
                            return -10;
                        }

                        r.Id = id;
                    }

                    process.Complete();
                }

                foreach (var r in reservations)
                {
                    _reservation.Add(r.Id, r);
                    _dateBooked.Add(r.DateTime, (int)ReservationStatus.Lock);
                }


            }

            return 0;
        }


        public void Dispose()
        {
        }



        internal List<ViewTimeSlot> InternalComputeTimeSlotHistory(List<RoomTimeSlot> timeSlots)
        {
            var list = new List<ViewTimeSlot>();

            foreach (var t in timeSlots)
            {
                string available = string.Empty;
                string booked = string.Empty;

                foreach (var time in t.Times.Split(','))
                {
                    var day = TimeSpan.Parse(time);

                    if (_dateBooked.ContainsKey(new DateTime(t.Date.Year, t.Date.Month, t.Date.Day, day.Hours, day.Minutes, day.Seconds)))
                    {
                        booked += $"{day.Hours}:{day.Minutes} ;";
                    }
                    else
                    {
                        available += $"{day.Hours}:{day.Minutes} ;";
                    }
                }

                var date = new ViewTimeSlot
                {
                    Id = t.Id,
                    RoomId = t.RoomId,
                    Date = t.Date.ToString("yyyy-MM-dd"),
                    AvailableTime = available,
                    BookedTime = booked,
                    Enable = t.Enable,
                    AbleToDelete = string.IsNullOrEmpty(booked) ? false : true,
                };

                list.Add(date);
            }

            return list;
        }

        public int DeleteTimeSlot(long TimeSlotId)
        {
            throw new NotImplementedException();
        }
    }
}
