using StudioReservation.BackOffice.Models;
using StudioReservation.Contract;
using StudioReservation.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
        //public ActionResult Index1()
        //{
        //    ViewBag.RoomId = "";
        //    ViewBag.StartDate = "";
        //    ViewBag.EndDateDate = "";
        //    var result = _reservationService.FindAllRoomTimeSlot();
        //    //if (result.Error != 0)
        //    //{
        //    //    ViewBag.ErrorCode = result.Error.ToString();
        //    //    return View("Error");
        //    //}
        //    List<string> roomList = new List<string>() { "Studio1", "Studio2"};
        //    ViewBag.MovieShow = new SelectList(roomList);

        //    return View(result);
        //}

        public ActionResult CreateGet(int roomId = 0)
        {
            CreateTimeSlotRequest model = new CreateTimeSlotRequest();

            var result = _reservationService.GetNotAvailableRoomDate(roomId);
            ViewBag.DisabledDate = result.NotAvailableDates;
            ViewBag.StartDate = result.StartTime;
            ViewBag.EndDate = result.EndTime;

            List<SelectListItem> roomList = new List<SelectListItem>();
            foreach(var i in result.Room)
            {
                roomList.Add(new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }

            List<SelectListItem> timeslots = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text = "00:00", Value = "00:00"},
                    new SelectListItem(){ Text = "01:00", Value = "01:00"},
                    new SelectListItem(){ Text = "02:00", Value = "02:00"},
                    new SelectListItem(){ Text = "03:00", Value = "03:00"},
                    new SelectListItem(){ Text = "04:00", Value = "04:00"},
                    new SelectListItem(){ Text = "05:00", Value = "05:00"},
                    new SelectListItem(){ Text = "06:00", Value = "06:00"},
                    new SelectListItem(){ Text = "07:00", Value = "07:00"},
                    new SelectListItem(){ Text = "08:00", Value = "08:00"},
                    new SelectListItem(){ Text = "09:00", Value = "09:00"},

                    new SelectListItem(){ Text = "10:00", Value = "10:00"},
                    new SelectListItem(){ Text = "11:00", Value = "11:00"},
                    new SelectListItem(){ Text = "12:00", Value = "12:00"},
                    new SelectListItem(){ Text = "13:00", Value = "13:00"},
                    new SelectListItem(){ Text = "14:00", Value = "14:00"},
                    new SelectListItem(){ Text = "15:00", Value = "15:00"},
                    new SelectListItem(){ Text = "16:00", Value = "16:00"},
                    new SelectListItem(){ Text = "17:00", Value = "17:00"},
                    new SelectListItem(){ Text = "18:00", Value = "18:00"},
                    new SelectListItem(){ Text = "19:00", Value = "19:00"},

                    new SelectListItem(){ Text = "20:00", Value = "20:00"},
                    new SelectListItem(){ Text = "21:00", Value = "21:00"},
                    new SelectListItem(){ Text = "22:00", Value = "22:00"},
                    new SelectListItem(){ Text = "23:00", Value = "23:00"}
                };
            ViewBag.Timeslots = new MultiSelectList(timeslots, "Value", "Text");

            if (roomId == 0)
            {                 
                ViewBag.RoomId = new SelectList(roomList, "Value", "Text");               
            }
            else
            {
                roomList.Where(x => x.Value ==  roomId.ToString()).First().Selected = true;
                ViewBag.RoomId = new SelectList(roomList, "Value", "Text");
            }
            return View(model);
        }
        [HttpPost]
        //public ActionResult Create([Bind(Include = "RoomId,Times,Enable,Dates")] CreateTimeSlotRequest request)
        public ActionResult Create(CreateTimeSlotRequest request)
        {

            var data = new CreateRoomTimeSlot
            {
                RoomId = request.RoomId,
                Dates = request.Dates,
                Times = request.Times,
                Enable = request.Enable,
                CreatedBy = "",
                CreateTime = DateTime.Now,
                UpdateBy = "",
                UpdateTime = DateTime.Now,
            };

            var result = _reservationService.CreateTimeSlot(data);
            if(result != 0)
            {
                ViewBag.ErrorCode = result.ToString();
                return View("Error");
            }

            return RedirectToAction("Index"); // 0 is success , other code is fail
        }
        [HttpGet]
        public ActionResult EditGet(long recordId)
        {
            var result = _reservationService.FindDetail(recordId);

            if(result.Error != 0)
            {
                ViewBag.ErrorCode = result.Error.ToString();
                return View("Error");
            }
            if (result.EnableEdit == false)
            {
                ViewBag.errorMessage = "You are not allowed to edit today or passed time slot.";
            }

            return View(result); 
        }

        [HttpPost]
        public ActionResult Edit(RoomTimeSlotDetail request)
        {

            var result = _reservationService.EditTimeSlot(request.Id, "request.Times", string.Empty, DateTime.Now, request.Enable);

            return View(result);

        }

        public ActionResult ConfirmDelete(string recordId = "")
        {
            var result = _reservationService.DeleteTimeSlot(long.Parse(recordId));

            if (result != 0)
            {
                ViewBag.ErrorCode = result.ToString();
                return View("Error");
            }

            return RedirectToAction("Index"); // 0 is success , other code is fail
        }

        //[HttpGet]
        //public ActionResult GetAll()
        //{

        //    var result = _reservationService.FindAllRoomTimeSlot();

        //    if (result.Error != 0)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(result);
        //}

        //[HttpGet]
        //public ActionResult Search(int roomId, DateTime startTime, DateTime endTime)
        //{

        //    var result = _reservationService.FindRoomTimeSlotByFilter(roomId, startTime, endTime);

        //    if (result.Error != 0)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(result);
        //}

        //[HttpGet]
        //public ActionResult FindDetail(long Id)
        //{

        //    var result = _reservationService.FindDetail(Id);

        //    if (result.Error != 0)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(result);
        //}

        //[HttpGet]
        //public ActionResult FindNotAvailableDate(int roomId)
        //{
        //    var result = _reservationService.GetNotAvailableRoomDate(roomId);

        //    return View(result);
        //}
    }
}