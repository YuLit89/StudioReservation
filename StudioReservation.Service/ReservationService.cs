﻿
using StudioReservation.Contract;
using StudioReservation.DataModel;
using StudioRoomType.DataModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
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

        Dictionary<long, RoomTimeSlot> _roomTimeSlot = new Dictionary<long, RoomTimeSlot>();

        Dictionary<int, Room> _roomType = new Dictionary<int, Room>();

        Dictionary<long, ReservationHistory> _reservation = new Dictionary<long, ReservationHistory>();
        Dictionary<DateTime, int> _dateBooked = new Dictionary<DateTime, int>();


        Func<RoomTimeSlot, long> _insertTimeSlot;
        Func<long, string, string, DateTime, bool, long> _updateTimeSlot;
        Func<ReservationHistory, long> _insertReservation;
        Func<long, int> _roomTimeSlotDelete;
        Func<long, int, DateTime, string, long> _updateReservationStatus;

        ConcurrentDictionary<int, object> _sync = new ConcurrentDictionary<int, object>();

        readonly int _timeSlotRange;

        public ReservationService(
            Func<IEnumerable<RoomTimeSlot>> getAllTimeSlot,
            Func<RoomTimeSlot, long> insertTimeSlot,
            Func<long, string, string, DateTime, bool, long> updateTimeSlot,
            Func<ReservationHistory, long> insertReservation,
            Func<IEnumerable<Room>> getAllRoomsType,
            int timeSlotRange,
            Func<long, int> roomTimeSlotDelete,
            Func<IEnumerable<ReservationHistory>> getAllReservation,
            Func<long,int,DateTime,string,long> updateReservationStatus)
        {
            _insertTimeSlot = insertTimeSlot;
            _updateTimeSlot = updateTimeSlot;

            _insertReservation = insertReservation;

            _timeSlotRange = timeSlotRange;
            _roomTimeSlotDelete = roomTimeSlotDelete;
            _updateReservationStatus = updateReservationStatus;

            Init(getAllTimeSlot(), getAllRoomsType(),getAllReservation());

        }

        internal void Init(IEnumerable<RoomTimeSlot> getAllTimeSlot, IEnumerable<Room> getRoomType,IEnumerable<ReservationHistory> getReservation)
        {
            foreach (var timeSlot in getAllTimeSlot)
            {
                if (timeSlot.isDeleted) continue;

                _roomTimeSlot.Add(timeSlot.Id, timeSlot);
            }

            foreach (var room in getRoomType)
            {
                _roomType.Add(room.Id, room);
            }

            foreach(var reservation in getReservation)
            {
                _reservation.Add(reservation.Id, reservation);
                _dateBooked.Add(reservation.DateTime, reservation.Status);
            }
        }

        public int CreateTimeSlot(CreateRoomTimeSlot request)
        {

            var timeSlots = new List<RoomTimeSlot>();

            var dates = request.Dates;

            using (var process = new TransactionScope())
            {
                foreach (var d in dates.Split(','))
                {
                    var date = DateTime.ParseExact(d, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var timeSlot = new RoomTimeSlot
                    {
                        Date = date,
                        RoomId = request.RoomId,
                        Times = string.Join(",", request.Times),
                        CreatedBy = request.CreatedBy,
                        CreateTime = request.CreateTime,
                        UpdateBy = request.UpdateBy,
                        UpdateTime = request.UpdateTime,
                        Enable = request.Enable,
                        isDeleted = false
                    };

                    var id = _insertTimeSlot(timeSlot);

                    if (id <= 0)
                    {
                        process.Dispose();

                        return -10;
                    }

                    timeSlot.Id = id;
                    timeSlots.Add(timeSlot);
                }

                process.Complete();
            }

            foreach (var slot in timeSlots)
            {
                _roomTimeSlot[slot.Id] = slot;
            }

            return 0;
        }

        public int EditTimeSlot(long TimeSlotId, string Times, string UpdateBy, DateTime UpdateTime, bool Enable)
        {
            RoomTimeSlot timeSlot;
            if (!_roomTimeSlot.TryGetValue(TimeSlotId, out timeSlot))
            {
                return -10;
            }

            if(UpdateTime.Date > timeSlot.Date)
            {
                return -11; // cant update previous date 
            }

            var newTimes = Times.Split(',').ToList();

            
            foreach(var time in timeSlot.Times.Split(','))
            {

                if (!newTimes.Contains(time))
                {
                    var oldTime = TimeSpan.Parse(time);

                    if (_dateBooked.ContainsKey(new DateTime(timeSlot.Date.Year, timeSlot.Date.Month, timeSlot.Date.Day, oldTime.Hours, oldTime.Minutes, oldTime.Seconds)))
                    {
                        return -12;  // time slot have ppl booked so not able to book
                    }
                }
            }

            var error = _updateTimeSlot(TimeSlotId, Times, UpdateBy, UpdateTime, Enable);

            if (error < 0)
            {
                return -13;
            }

            timeSlot.Times = Times;
            timeSlot.UpdateTime = UpdateTime;
            timeSlot.UpdateBy = UpdateBy;
            timeSlot.Enable = Enable;

            return 0;
        }

        public ViewAllTimeSlot FindAllRoomTimeSlot()
        {
            var now = DateTime.Now;


            var timeSlots = _roomTimeSlot.Values.Where(x => !x.isDeleted).Where(x => x.Date >= now.Date).OrderBy(x => x.Id).ToList();

            var history = InternalComputeTimeSlotHistory(timeSlots.ToList());

            return new ViewAllTimeSlot
            {
                TimeSlots = history,
                Error = 0
            };
        }

        //public ViewAllTimeSlot FindRoomTimeSlotByFilter(int RoomId, DateTime StartDate, DateTime EndDate)
        //{
        //    var now = DateTime.Now;

        //    IEnumerable<RoomTimeSlot> query;

        //    if(RoomId != 0)
        //    {
        //        query = _roomTimeSlot.Where(x => x.Value.RoomId == RoomId).Select(x => x.Value);
        //    }

        //    var timeSlots = _roomTimeSlot.Values.Where(x => x.Date >= StartDate && x.Date <= EndDate).OrderBy(x => x.Id).ToList();

        //    var history = InternalComputeTimeSlotHistory(timeSlots.ToList());

        //    return new ViewAllTimeSlot
        //    {
        //        TimeSlots = history,
        //        Error = 0

        //    };
        //}

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
                    timeStatus.Add(day.ToString(@"hh\:mm"), (int)ReservationStatus.Opening);
                }
                else
                {
                    if (status == (int)ReservationStatus.Booked) timeStatus.Add(day.ToString(@"hh\:mm"), (int)ReservationStatus.Booked);
                    if (status == (int)ReservationStatus.Lock) timeStatus.Add(day.ToString(@"hh\:mm"), (int)ReservationStatus.Lock);
                }
            }

            return new RoomTimeSlotDetail()
            {
                Id = info.Id,
                RoomId = info.RoomId,
                RoomName = room.Name,
                Enable = info.Enable,
                Date = info.Date.ToString("yyyy-MM-dd"),
                Times = timeStatus,
                RoomTypeImage = room.Image,
                EnableEdit = (now.Date > info.Date.Date) ? false : true,
                CreatedTime = info.CreateTime,
                CreatedBy = info.CreatedBy,
                UpdateBy = info.UpdateBy,
                UpdateTime = info.UpdateTime,
                Error = 0
            };
        }

        public NotAvailableRoomDate GetNotAvailableRoomDate(int RoomId)
        {

            var now = DateTime.Now.Date;
            var endDate = now.AddDays(_timeSlotRange);

            var room = new List<RoomType>();

            foreach (var r in _roomType.Values)
            {
                var r1 = new RoomType { Id = r.Id, Name = r.Name };

                room.Add(r1);
            }

            var i = _roomTimeSlot.Values.Where(x => x.RoomId == RoomId).Where(x => x.Date >= now && x.Date <= endDate).Select(x => x.Date.ToString("dd/MM/yyyy")).Distinct().ToList();

            return new NotAvailableRoomDate
            {
                Room = room,
                NotAvailableDates = string.Join(",", i),
                StartTime = now.ToString("dd/MM/yyyy"),
                EndTime = endDate.ToString("dd/MM/yyyy"),
                Error = 0,
            };
        }

        public int LockTimeSlotReservation(TimeSlotReservationRequest Request)
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
                        Status = (int)ReservationStatus.Lock,
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
                        booked += $"{day.ToString(@"hh\:mm")} ;";
                    }
                    else
                    {
                        available += $"{day.ToString(@"hh\:mm")} ;";
                    }
                }

                Room room;
                _roomType.TryGetValue(t.RoomId, out room);

                var date = new ViewTimeSlot
                {
                    Id = t.Id,
                    RoomId = t.RoomId,
                    Date = t.Date.ToString("yyyy-MM-dd"),
                    AvailableTime = available,
                    BookedTime = booked,
                    Enable = t.Enable,
                    RoomName = room?.Name ?? string.Empty,
                    AbleToDelete = string.IsNullOrEmpty(booked) ? true : false,
                };

                list.Add(date);
            }

            return list;
        }

        public int DeleteTimeSlot(long TimeSlotId)
        {
            RoomTimeSlot timeInfo;

            if (_roomTimeSlot.TryGetValue(TimeSlotId, out timeInfo))
            {

                foreach (var time in timeInfo.Times.Split(','))
                {
                    var day = TimeSpan.Parse(time);

                    if (_dateBooked.ContainsKey(new DateTime(timeInfo.Date.Year, timeInfo.Date.Month, timeInfo.Date.Day, day.Hours, day.Minutes, day.Seconds)))
                    {
                        return -10;
                    }
                }
            }

            var delete = _roomTimeSlotDelete(TimeSlotId);

            if (delete == 0)
            {
                _roomTimeSlot.Remove(TimeSlotId);
            }

            return 0;

        }

        public ScheduleViewModel ReservationSchedule(long RoomId, string selectedDate = null)
        {
            var model = new ScheduleViewModel();

            var date = selectedDate == null ? DateTime.Now.Date : DateTime.ParseExact(selectedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
         
            var schedule = new List<ScheduleDate>();

            var timeSlots = _roomTimeSlot.Values.Where(x=> x.RoomId == RoomId).Where(x => !x.isDeleted).Where(x => x.Date >= date).Where(x => x.Date < date.AddDays(7)).OrderBy(x=> x.Date);


            for(int i = 0; i < 7; i++)
            {
                 var newDate = date.AddDays(i);
                 var timeInfo = _roomTimeSlot.Values.Where(x => x.Date == newDate).Where(x => !x.isDeleted).FirstOrDefault();

                if (timeInfo == null)
                {
                    schedule.Add(new ScheduleDate
                    {
                        Enable = false,
                        Date = newDate.ToString("dd/MM/yyyy"),
                        roomStatuses = new List<RoomStatus>()
                    });
                }
                else
                {
                    var dateTimes = timeInfo.Times.GenerateDateTime(newDate);

                    var times = new Dictionary<TimeSpan,int>();

                    foreach (var time in dateTimes)
                    {
                        int status;
                        if (_dateBooked.TryGetValue(time, out status))
                        {
                            times.Add(new TimeSpan(time.Hour, time.Minute, time.Second),status);
                        }
                        else
                        {
                            times.Add(new TimeSpan(time.Hour, time.Minute, time.Second),(int)ReservationStatus.Opening);
                        }
                    }

                    var fullTimes = Compute24HourTime(times);

                    var s = new ScheduleDate()
                    {
                        Date = newDate.ToString("dd/MM/yyyy"),
                        Enable = true,
                        roomStatuses = fullTimes
                    };

                    schedule.Add(s);
                }
            }
      
            model.DisplayDate = date.ToString("dd/MM/yyyy");
            model.scheduleDates = schedule;
            model.ErrorCode = 0;

            return model;
        }

        public int UpdateSuccessReservation(long ReservationId)
        {
            var now = DateTime.Now;

            ReservationHistory history;
            if (!_reservation.TryGetValue(ReservationId,out history))
            {
                return -10;
            }

            if(history.Status == (int)ReservationStatus.Booked)
            {
                return 0;
            }

            history.Status = (int)ReservationStatus.Booked;
            history.UpdateTime = DateTime.Now;

            var err = _updateReservationStatus(history.Id, (int)ReservationStatus.Booked, now, string.Empty);

            if(err <= 0)
            {
                return -11;
            }

            history.Status = (int)ReservationStatus.Booked;
            history.UpdateTime = DateTime.Now;

            _dateBooked[history.DateTime] = (int)ReservationStatus.Booked;

            return 0;
            
        }

        internal List<RoomStatus> Compute24HourTime(IDictionary<TimeSpan, int> RoomStatus)
        {

            var result = new List<RoomStatus>();
            for (int i = 0; i < 24; i++)
            {
                var time = new TimeSpan(i, 00, 00);

                int status;
                if (!RoomStatus.TryGetValue(time, out status))
                {
                    RoomStatus.Add(time, (int)ReservationStatus.Closed);
                }
            }

            var item = RoomStatus.OrderBy(x => x.Key);

            foreach (var i in item)
            {
                result.Add(new DataModel.RoomStatus
                {
                    timeslot = i.Key.ToString(@"hhmm"),
                    status = i.Value.ToString()
                });
            };


            return result;
        }

    }

    public static class Extension
    {
        public static List<DateTime> GenerateDateTime(this string times ,DateTime date)
        {

            var dts = new List<DateTime>();

            foreach(var time in times.Split(','))
            {
                var t = TimeSpan.Parse(time);

                dts.Add(new DateTime(date.Year, date.Month, date.Day, t.Hours, t.Minutes, t.Seconds));
            }

            

            return dts;
        }
    }
}
