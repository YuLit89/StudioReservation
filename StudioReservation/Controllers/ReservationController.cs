using StudioReservation.Contract;
using StudioReservation.Models;
using StudioRoomType.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudioReservation.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reservation
        readonly IReservationService _reservationService;
        public ReservationController()
        {
            _reservationService = ServiceConnection.ReservationService;
        }

        public ActionResult Index()
        {
            RoomViewModels roomViewModel = new RoomViewModels()
            {
                Rooms = new List<Room>
              {
                  new Room {
                    Id =  1,
                    Name = "Studio 1",
                    Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
                    Image = "Studio1.jpg",
                    Size = "800 * 750",
                    Style= "K-pop;Hip Hop;Jazz",
                    Rate = 100.00M
                  },
                  new Room {
                    Id =  2,
                    Name = "Studio 2",
                    Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
                    Image = "Studio2.jpg",
                    Size = "800 * 750",
                    Style= "K-pop;Hip Hop;Jazz",
                    Rate = 120.00M
                  },
                  new Room {
                    Id =  3,
                    Name = "Studio 3",
                    Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
                    Image = "Studio3.jpg",
                    Size = "800 * 750",
                    Style= "K-pop;Hip Hop;Jazz",
                    Rate = 100.00M
                  },
                  new Room {
                    Id =  4,
                    Name = "Studio 4",
                    Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
                    Image = "Studio4.jpg",
                    Size = "800 * 750",
                    Style= "K-pop;Hip Hop;Jazz",
                    Rate = 100.00M
                  },
                  new Room {
                    Id =  5,
                    Name = "Studio 5",
                    Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
                    Image = "Studio5.jpg",
                    Size = "800 * 750",
                    Style= "K-pop;Hip Hop;Jazz",
                    Rate = 100.00M
                  }
              }
            };

            return View(roomViewModel);
        }

        public ActionResult Schedule(long id, string selectedDate = null)
        {
            ViewBag.RoomId = id;

            var result = _reservationService.ReservationSchedule(id, selectedDate);
            //var model = new ScheduleViewModel();
            //model.DisplayDate = selectedDate == null ? DateTime.Now.AddDays(-1).Date.ToString("dd/MM/yyyy") : selectedDate;
            //model.scheduleDates = new List<ScheduleDate>() {
            //            new ScheduleDate(){
            //                Date = "17/07/2024",
            //                Enable = true,
            //                roomStatuses = new List<RoomStatus>(){
            //                    new RoomStatus(){ timeslot = "0800", status = "1"},
            //                    new RoomStatus(){ timeslot = "0900", status = "1"},
            //                    new RoomStatus(){ timeslot = "1000", status = "1"},
            //                    new RoomStatus(){ timeslot = "1100", status = "2"},
            //                    new RoomStatus(){ timeslot = "1200", status = "2"},
            //                    new RoomStatus(){ timeslot = "1300", status = "1"},
            //                    new RoomStatus(){ timeslot = "1400", status = "1"},
            //                    new RoomStatus(){ timeslot = "1500", status = "1"},
            //                    new RoomStatus(){ timeslot = "1600", status = "1"},
            //                    new RoomStatus(){ timeslot = "1700", status = "1"},
            //                    new RoomStatus(){ timeslot = "1800", status = "1"},
            //                    new RoomStatus(){ timeslot = "1900", status = "2"},
            //                    new RoomStatus(){ timeslot = "2000", status = "1"},
            //                    new RoomStatus(){ timeslot = "2100", status = "1"}
            //                } },
            //            new ScheduleDate(){
            //                Date = "18/07/2024",
            //                Enable = true,
            //                roomStatuses = new List<RoomStatus>(){
            //                    new RoomStatus(){ timeslot = "0800", status = "1"},
            //                    new RoomStatus(){ timeslot = "0900", status = "1"},
            //                    new RoomStatus(){ timeslot = "1000", status = "1"},
            //                    new RoomStatus(){ timeslot = "1100", status = "1"},
            //                    new RoomStatus(){ timeslot = "1200", status = "1"},
            //                    new RoomStatus(){ timeslot = "1300", status = "1"},
            //                    new RoomStatus(){ timeslot = "1400", status = "2"},
            //                    new RoomStatus(){ timeslot = "1500", status = "2"},
            //                    new RoomStatus(){ timeslot = "1600", status = "1"},
            //                    new RoomStatus(){ timeslot = "1700", status = "1"},
            //                    new RoomStatus(){ timeslot = "1800", status = "1"},
            //                    new RoomStatus(){ timeslot = "1900", status = "1"},
            //                    new RoomStatus(){ timeslot = "2000", status = "1"},
            //                    new RoomStatus(){ timeslot = "2100", status = "2"}
            //                }
            //            },
            //            new ScheduleDate(){
            //                Date = "19/07/2024",
            //                Enable = true,
            //                roomStatuses = new List<RoomStatus>(){
            //                    new RoomStatus(){ timeslot = "0800", status = "2"},
            //                    new RoomStatus(){ timeslot = "0900", status = "2"},
            //                    new RoomStatus(){ timeslot = "1000", status = "1"},
            //                    new RoomStatus(){ timeslot = "1100", status = "1"},
            //                    new RoomStatus(){ timeslot = "1200", status = "1"},
            //                    new RoomStatus(){ timeslot = "1300", status = "1"},
            //                    new RoomStatus(){ timeslot = "1400", status = "1"},
            //                    new RoomStatus(){ timeslot = "1500", status = "1"},
            //                    new RoomStatus(){ timeslot = "1600", status = "1"},
            //                    new RoomStatus(){ timeslot = "1700", status = "2"},
            //                    new RoomStatus(){ timeslot = "1800", status = "1"},
            //                    new RoomStatus(){ timeslot = "1900", status = "1"},
            //                    new RoomStatus(){ timeslot = "2000", status = "1"},
            //                    new RoomStatus(){ timeslot = "2100", status = "1"}
            //                }
            //            },
            //            new ScheduleDate(){
            //                Date = "20/07/2024",
            //                Enable = true,
            //                roomStatuses = new List<RoomStatus>(){
            //                    new RoomStatus(){ timeslot = "0800", status = "1"},
            //                    new RoomStatus(){ timeslot = "0900", status = "1"},
            //                    new RoomStatus(){ timeslot = "1000", status = "1"},
            //                    new RoomStatus(){ timeslot = "1100", status = "1"},
            //                    new RoomStatus(){ timeslot = "1200", status = "2"},
            //                    new RoomStatus(){ timeslot = "1300", status = "2"},
            //                    new RoomStatus(){ timeslot = "1400", status = "2"},
            //                    new RoomStatus(){ timeslot = "1500", status = "2"},
            //                    new RoomStatus(){ timeslot = "1600", status = "1"},
            //                    new RoomStatus(){ timeslot = "1700", status = "1"},
            //                    new RoomStatus(){ timeslot = "1800", status = "1"},
            //                    new RoomStatus(){ timeslot = "1900", status = "1"},
            //                    new RoomStatus(){ timeslot = "2000", status = "2"},
            //                    new RoomStatus(){ timeslot = "2100", status = "1"}
            //                }
            //            },
            //            new ScheduleDate(){
            //                Date = "21/07/2024",
            //                Enable = true,
            //                roomStatuses = new List<RoomStatus>(){
            //                    new RoomStatus(){ timeslot = "0800", status = "1"},
            //                    new RoomStatus(){ timeslot = "0900", status = "1"},
            //                    new RoomStatus(){ timeslot = "1000", status = "1"},
            //                    new RoomStatus(){ timeslot = "1100", status = "1"},
            //                    new RoomStatus(){ timeslot = "1200", status = "2"},
            //                    new RoomStatus(){ timeslot = "1300", status = "2"},
            //                    new RoomStatus(){ timeslot = "1400", status = "2"},
            //                    new RoomStatus(){ timeslot = "1500", status = "2"},
            //                    new RoomStatus(){ timeslot = "1600", status = "1"},
            //                    new RoomStatus(){ timeslot = "1700", status = "1"},
            //                    new RoomStatus(){ timeslot = "1800", status = "3"},
            //                    new RoomStatus(){ timeslot = "1900", status = "1"},
            //                    new RoomStatus(){ timeslot = "2000", status = "2"},
            //                    new RoomStatus(){ timeslot = "2100", status = "1"}
            //                }
            //            },
            //            new ScheduleDate(){
            //                Date = "22/07/2024",
            //                Enable = true,
            //                roomStatuses = new List<RoomStatus>(){
            //                    new RoomStatus(){ timeslot = "0800", status = "1"},
            //                    new RoomStatus(){ timeslot = "0900", status = "1"},
            //                    new RoomStatus(){ timeslot = "1000", status = "1"},
            //                    new RoomStatus(){ timeslot = "1100", status = "1"},
            //                    new RoomStatus(){ timeslot = "1200", status = "2"},
            //                    new RoomStatus(){ timeslot = "1300", status = "2"},
            //                    new RoomStatus(){ timeslot = "1400", status = "2"},
            //                    new RoomStatus(){ timeslot = "1500", status = "2"},
            //                    new RoomStatus(){ timeslot = "1600", status = "1"},
            //                    new RoomStatus(){ timeslot = "1700", status = "1"},
            //                    new RoomStatus(){ timeslot = "1800", status = "1"},
            //                    new RoomStatus(){ timeslot = "1900", status = "1"},
            //                    new RoomStatus(){ timeslot = "2000", status = "2"},
            //                    new RoomStatus(){ timeslot = "2100", status = "1"}
            //                }
            //            },
            //            new ScheduleDate(){
            //                Date = "23/07/2024",
            //                Enable = false,
            //                roomStatuses = new List<RoomStatus>(){
            //                    new RoomStatus(){ timeslot = "0800", status = "1"},
            //                    new RoomStatus(){ timeslot = "0900", status = "1"},
            //                    new RoomStatus(){ timeslot = "1000", status = "1"},
            //                    new RoomStatus(){ timeslot = "1100", status = "1"},
            //                    new RoomStatus(){ timeslot = "1200", status = "2"},
            //                    new RoomStatus(){ timeslot = "1300", status = "2"},
            //                    new RoomStatus(){ timeslot = "1400", status = "2"},
            //                    new RoomStatus(){ timeslot = "1500", status = "2"},
            //                    new RoomStatus(){ timeslot = "1600", status = "1"},
            //                    new RoomStatus(){ timeslot = "1700", status = "1"},
            //                    new RoomStatus(){ timeslot = "1800", status = "1"},
            //                    new RoomStatus(){ timeslot = "1900", status = "1"},
            //                    new RoomStatus(){ timeslot = "2000", status = "2"},
            //                    new RoomStatus(){ timeslot = "2100", status = "1"}
            //                }
            //            },
            //        };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReservationConfirm(string roomId = null, string selectedSlots = null)
        {
            if (string.IsNullOrEmpty(selectedSlots))
            {
                ViewBag.errorMessage = "You did not select any timeslot.";
                return View("Error");
            }

            ReservationConfirmViewModel reservationConfirmViewModel = new ReservationConfirmViewModel();
            reservationConfirmViewModel.roomName = roomId;

            List<string> selectedList = selectedSlots.Split(',').ToList();
            selectedList.Sort();
            reservationConfirmViewModel.orders = selectedList.GroupBy(s => s.Substring(0, 10),
                                 s => s.Substring(s.Length - 4),
                                 (a, b) => new Order { date = a, slots = b.ToList(), fee = b.Count() * 110.00M }).ToList();

            reservationConfirmViewModel.termCondition = new List<string>()
            {
                "Please double check your booking details.",
                "No refund or cancellation allowed after booking is made."
            };
            decimal rentalCost = 0;
            foreach (var item in reservationConfirmViewModel.orders)
            {
                rentalCost += item.fee;
            }
            reservationConfirmViewModel.rentalCost = rentalCost;
            reservationConfirmViewModel.platformCharge = 1.00M;
            reservationConfirmViewModel.totalAmount = reservationConfirmViewModel.rentalCost + reservationConfirmViewModel.platformCharge;
            ViewBag.Timer = "20";
            string timerDisplay = TimeSpan.FromSeconds(20).ToString(@"mm\:ss");
            ViewBag.TimerDisplay = timerDisplay;
            ViewBag.RoomId = roomId;
            ViewBag.selectedSlots = selectedSlots;
            return View(reservationConfirmViewModel);
        }

        public ActionResult ReservationCancel(string roomId = null, string selectedSlots = null)
        {
            string o = "";
            //cancel current booking.

            return RedirectToAction("Index", "Reservation");

        }
    }
}