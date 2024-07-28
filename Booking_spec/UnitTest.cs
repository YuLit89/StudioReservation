using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudioReservation.Contract;
using StudioReservation.DataModel;
using StudioReservation.Proxy;
using StudioRoomType.DataModel;
using System;
using System.Collections.Generic;
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
                Dates = "2024-07-25,2024-07-26,2024-07-27",
                Times = new string[] {"13:00:00","14:00:00","15:00:00","16:00:00","20:00:00","21:00:00"},
                CreatedBy = string.Empty,
                CreateTime = DateTime.Now,
                RoomId = 1,
                Enable = true,
                UpdateBy = string.Empty,
                UpdateTime = DateTime.Now
            });

            Assert.AreEqual(0, result);

            var result1 = service.CreateTimeSlot(new CreateRoomTimeSlot
            {
                Dates = "2024-08-01,2024-08-02,2024-08-03,2024-08-04,2024-08-05,2024-08-06,2024-08-07,2024-08-08,2024-08-09,2024-08-10,2024-08-11,2024-08-12,2024-08-13,2024-08-14,2024-08-15,2024-08-16",
                Times =  new string[] {"06:00:00","07:00:00","08:00:00","09:00:00"},
                CreatedBy = string.Empty,
                CreateTime = DateTime.Now,
                RoomId = 2,
                Enable = true,
                UpdateBy = string.Empty,
                UpdateTime = DateTime.Now
            });

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
            Assert.AreEqual(9, result.AvailableTime.Count());
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
            string v = "13:00";

            TimeSpan v1 = TimeSpan.Parse(v);
        }
    }
}
