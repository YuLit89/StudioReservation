using StudioReservation.Contract;
using StudioReservation.DataModel;
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

        Dictionary<long,ReservationHistory> _reservation = new Dictionary<long,ReservationHistory>();
        Dictionary<DateTime, int> _dateBooked = new Dictionary<DateTime, int>();

        ConcurrentDictionary<int, object> _sync = new ConcurrentDictionary<int, object>();

        Func<RoomTimeSlot, long> _insertTimeSlot;
        Func<long, string, string, DateTime, bool, long> _updateTimeSlot;
        Func<ReservationHistory, long> _insertReservation;


        public ReservationService(
            Func<RoomTimeSlot, long> insertTimeSlot,
            Func<long,string,string,DateTime,bool,long> updateTimeSlot,
            Func<ReservationHistory,long> insertReservation)
        {
            _insertTimeSlot = insertTimeSlot;
            _updateTimeSlot = updateTimeSlot;

            _insertReservation = insertReservation;
        }

        public void Dispose()
        {
        }

        public int CreateTimeSlot(RoomTimeSlotRequest request)
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

        public int UpdateTimeSlot(long TimeSlotId, string Times, string UpdateBy, DateTime UpdateTime, bool Enable)
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

            var all = new List<ViewTimeSlot>();

            int totelPage = _roomTimeSlot.Values.Where(x => x.Date >= now.Date).Count()/Size;

            var sizeTaken = (LastId == 0) ? 0 : Size * (Page - 1);

            var timeSlot = (LastId == 0)
                           ? _roomTimeSlot.Values.Where(x => x.Date >= now.Date).OrderBy(x => x.Id).Take(sizeTaken)
                           : _roomTimeSlot.Values.Where(x => x.Id > LastId).OrderBy(x => x.Id).Take(sizeTaken).Skip(sizeTaken-Size);

            foreach(var t in timeSlot)
            {
                var dateHaveBooked = _dateBooked.ContainsKey(t.Date);

                var date = new ViewTimeSlot
                {
                    Id = t.Id,
                    RoomId = t.RoomId,
                    Date = t.Date,
                    Times = t.Times,
                    Enable = t.Enable,
                    AbleToDelete = !dateHaveBooked  
                };

                all.Add(date);
            }

            return new ViewAllTimeSlot
            {
                TimeSlots = all,
                Error = 0,
                TotalPage = totelPage,
                Paging = (Page == 0 ) ? 1 : Page,
                LastId = (all.Count() > 0) ? all.Last().Id : 0
            };
        }

        public ViewAllTimeSlot FindRoomTimeSlotByFilter(int RoomId, DateTime StartDate, DateTime EndDate, int Page, long LastId, int Size = 20)
        {
            var now = DateTime.Now;

            var all = new List<ViewTimeSlot>();

            IEnumerable<RoomTimeSlot> query;

            if(RoomId != 0)
            {
                query = _roomTimeSlot.Where(x => x.Value.RoomId == RoomId).Select(x => x.Value);
            }

            
                var timeSlot = (LastId == 0)
                           ? _roomTimeSlot.Values.Where(x => x.Date >= StartDate && x.Date <= EndDate).OrderBy(x => x.Id).Take(Size)
                           : _roomTimeSlot.Values.Where(x => x.Id > LastId).OrderBy(x => x.Id).Take(Size);

            foreach (var t in timeSlot)
            {
                var dateHaveBooked = _dateBooked.ContainsKey(t.Date);

                var date = new ViewTimeSlot
                {
                    Id = t.Id,
                    RoomId = t.RoomId,
                    Date = t.Date,
                    Times = t.Times,
                    Enable = t.Enable,
                    AbleToDelete = !dateHaveBooked
                };

                all.Add(date);
            }

            return new ViewAllTimeSlot
            {
                TimeSlots = all,
                Error = 0,
                TotalPage = (timeSlot.Count() / 2),
                Paging = Page + 1,
                LastId = (all.Count() > 0) ? all.Last().Id : 0
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

                    foreach(var r in reservations)
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

                foreach(var r in reservations)
                {
                    _reservation.Add(r.Id, r);
                    _dateBooked.Add(r.DateTime, (int)ReservationStatus.Lock);
                }


            }

            return 0;
        }

        public ViewTimeSlotDetail FindRoomTimeSlotDetail(long Id)
        {
            throw new NotImplementedException();
        }
    }
}
