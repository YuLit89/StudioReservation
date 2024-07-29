using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudioReservation.Contract;
using StudioReservation.DataModel;
using StudioReservation.Proxy;
using StudioRoomType.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_spec
{
    [TestClass]
    public class UnitTest
    {
        IReservationService service;

        [TestInitialize]
        public void init()
        {
             service = new ReservationServiceProxy(1001);
        }

        [TestMethod]
        public void TimeParse()
        {
            string time = "13:00:00";

            TimeSpan t = TimeSpan.Parse(time);

            DateTime t1 = DateTime.Parse(time);

            string date = "2024-05-20";

            DateTime t2 = DateTime.Parse(date);

        }

        [TestMethod]
        public void CallProxy_create()
        {

            var result = service.CreateTimeSlot(new CreateRoomTimeSlot
            {
                Dates = "20/08/2024,21/08/2024",
                Times = new string[] { "13:00:00", "14:00:00", "15:00:00", "16:00:00", "20:00:00", "21:00:00" },
                CreatedBy = string.Empty,
                CreateTime = DateTime.Now,
                RoomId = 1,
                Enable = true,
                UpdateBy = string.Empty,
                UpdateTime = DateTime.Now
            });

            Assert.AreEqual(0, result);

            //var result1 = service.CreateTimeSlot(new CreateRoomTimeSlot
            //{
            //    Dates = "2024-08-01,2024-08-02,2024-08-03,2024-08-04,2024-08-05,2024-08-06,2024-08-07,2024-08-08,2024-08-09,2024-08-10,2024-08-11,2024-08-12,2024-08-13,2024-08-14,2024-08-15,2024-08-16",
            //    Times =  new string[] {"06:00:00","07:00:00","08:00:00","09:00:00"},
            //    CreatedBy = string.Empty,
            //    CreateTime = DateTime.Now,
            //    RoomId = 2,
            //    Enable = true,
            //    UpdateBy = string.Empty,
            //    UpdateTime = DateTime.Now
            //});

            //Assert.AreEqual(0, result1);

        }

        [TestMethod]
        public void tiemslot_GetHistory()
        {
        
            var result = service.FindAllRoomTimeSlot();

        }

        [TestMethod]
        public void find_timeslot_detail()
        {

            var result = service.FindDetail(27);

            Assert.AreEqual(0, result.Error);
            //Assert.AreEqual(9, result.AvailableTime.Count());
            Assert.AreEqual(new DateTime(2024, 08, 16),result.Date);
        }

        [TestMethod]
        public void findNotAvailableDate()
        {

            var result = service.GetNotAvailableRoomDate(1);

        }

        [TestMethod]
        public void x()
        {
            string xx = "13:00:00";

            var day = TimeSpan.Parse(xx);

            var xxx = day.ToString(@"hh\:mm");

        }

        [TestMethod]
        public void get_null_room()
        {
            Room room = null;

            string v = room?.Name ?? string.Empty;
        }

        [TestMethod]
        public void delete()
        {
            var delete = service.DeleteTimeSlot(37);
        }

        [TestMethod]
        public void timespan_convert()
        {
            string v = "20/08/2024";

            DateTime dt = DateTime.ParseExact(v, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //DateTime v1 = Convert.ToDateTime(v);
        }

        [TestMethod]
        public void findDetail()
        {
            var result = service.FindDetail(21);
        }

        [TestMethod]
        public void edit()
        {
            var result = service.EditTimeSlot(30, "01:00,02:00,03:00,04:00,10:00,20:00", "", DateTime.Now, true);
        }

        [TestMethod]
        public void lock_reservation()
        {

            var result = service.LockTimeSlotReservation(new TimeSlotReservationRequest
            {
                MemberId = string.Empty,
                RoomId = 2,
                ReservationTime = "2024-08-04 06:00:00,2024-08-05 09:00:00"
            });

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void update_success_reservation()
        {

            var result = service.UpdateSuccessReservation(1);

            Assert.AreEqual(0, result);

            var view = service.FindDetail(20);

            var result2 = service.UpdateSuccessReservation(2);

            Assert.AreEqual(0, result2);

            var view1 = service.FindDetail(21);
        }
    }
}
