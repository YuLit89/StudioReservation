using StudioReservation.BackOffice.Models;
using StudioReservation.Contract;
using StudioReservation.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudioReservation.BackOffice.Controllers
{
    public class TimeSlotController : Controller
    {
        public IReservationService _reservationService;

        public TimeSlotController()
        {
            _reservationService = ServiceConnection._reservationService;
        }        
        public ActionResult Index()
        {
            var result = _reservationService.FindAllRoomTimeSlot();
            if (result.Error != 0)
            {
                ViewBag.ErrorCode = result.Error.ToString();
                return View("Error");
            }
            return View(result);
        }
        public ActionResult Index1()
        {
            ViewBag.RoomId = "";
            ViewBag.StartDate = "";
            ViewBag.EndDateDate = "";
            var result = _reservationService.FindAllRoomTimeSlot();
            //if (result.Error != 0)
            //{
            //    ViewBag.ErrorCode = result.Error.ToString();
            //    return View("Error");
            //}
            List<string> roomList = new List<string>() { "Studio1", "Studio2"};
            ViewBag.MovieShow = new SelectList(roomList);

            return View(result);
        }

        public ActionResult CreateGet(string roomId = "")
        {
            CreateTimeSlotRequest model = new CreateTimeSlotRequest();
            List<SelectListItem> roomList = new List<SelectListItem>()
            { new SelectListItem(){ Text="Studio1", Value="1"},
              new SelectListItem(){ Text="Studio2", Value="2"}
            };
            List<SelectListItem> timeslots = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text = "00:00", Value = "00:00"},
                    new SelectListItem(){ Text = "01:00", Value = "01:00"},
                    new SelectListItem(){ Text = "02:00", Value = "02:00"},
                    new SelectListItem(){ Text = "03:00", Value = "03:00"},
                    new SelectListItem(){ Text = "04:00", Value = "04:00"},
                    new SelectListItem(){ Text = "05:00", Value = "05:00"}
                };
            ViewBag.Timeslots = new MultiSelectList(timeslots, "Value", "Text");

            if (string.IsNullOrEmpty(roomId))
            {                 
                ViewBag.RoomId = new SelectList(roomList, "Value", "Text");
                
            }
            else
            {
                roomList.Where(x => x.Value == roomId).First().Selected = true;
                ViewBag.RoomId = new SelectList(roomList, "Value", "Text");
            }
            return View(model);



        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "RoomId,Times,Enable,Dates")] CreateTimeSlotRequest request)
        {

            var data = new CreateRoomTimeSlot
            {
                RoomId = request.RoomId,
                Dates = request.Dates,
               // Times = request.Times,
                Enable = request.Enable,
                CreatedBy = "",
                CreateTime = DateTime.Now,
                UpdateBy = "",
                UpdateTime = DateTime.Now,
            };

            var result = _reservationService.CreateTimeSlot(data);

            ModelState.AddModelError(result.ToString(), "Success");
            return View("Error"); // 0 is success , other code is fail
        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,RoomId,Times,Enable")] EditTimeSlot request)
        {

            var result = _reservationService.EditTimeSlot(request.Id, request.Times, string.Empty, DateTime.Now, request.Enable);

            return View(result);

        }

        [HttpGet]
        public ActionResult GetAll()
        {

            var result = _reservationService.FindAllRoomTimeSlot();

            if (result.Error != 0)
            {
                return HttpNotFound();
            }

            return View(result);
        }

        [HttpGet]
        public ActionResult Search(int roomId, DateTime startTime, DateTime endTime)
        {

            var result = _reservationService.FindRoomTimeSlotByFilter(roomId, startTime, endTime);

            if (result.Error != 0)
            {
                return HttpNotFound();
            }

            return View(result);
        }

        [HttpGet]
        public ActionResult FindDetail(long Id)
        {

            var result = _reservationService.FindDetail(Id);

            if (result.Error != 0)
            {
                return HttpNotFound();
            }

            return View(result);
        }

        [HttpGet]
        public ActionResult FindNotAvailableDate(int roomId)
        {
            var result = _reservationService.GetNotAvailableRoomDate(roomId);

            return View(result);
        }
    }
}