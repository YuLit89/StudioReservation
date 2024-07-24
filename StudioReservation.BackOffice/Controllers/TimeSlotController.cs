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

        [HttpPost]
        public ActionResult Create([Bind(Include ="RoomId,Times,Enable,Dates")] CreateTimeSlot request)
        {

            var data = new RoomTimeSlotRequest
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

            return View(result); // 0 is success , other code is fail
        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,RoomId,Times,Enable")] EditTimeSlot request)
        {

            var result = _reservationService.UpdateTimeSlot(request.Id,request.Times,string.Empty,DateTime.Now,request.Enable);

            return View(result);

        }

        [HttpGet]
        public ActionResult GetAll(int page, long lastId)
        {

            var result = _reservationService.FindAllRoomTimeSlot(page,lastId);

            if(result.Error != 0)
            {
                return HttpNotFound();
            }

            return View(result);
        }

        [HttpGet]
        public ActionResult Search(int roomId,DateTime startTime , DateTime endTime ,int page, long lastId)
        {

            var result = _reservationService.FindRoomTimeSlotByFilter(roomId,startTime,endTime,page, lastId);

            if (result.Error != 0)
            {
                return HttpNotFound();
            }

            return View(result);
        }
    }
}