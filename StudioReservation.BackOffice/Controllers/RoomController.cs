using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StudioMiscellaneous.Service.Contract;

//using StudioReservation.DataModel;
using StudioRoomType.DataModel;

namespace StudioReservation.BackOffice.Controllers
{
    public class RoomController : Controller
    {
        IStudioMiscellaneousService _miscellaneousSerice;
        public RoomController()
        {
            _miscellaneousSerice = ServiceConnection.MiscellaneousSevice;
        }

        public ActionResult Index()

        {
            //var result = _reservationService.FindAllRoomTimeSlot();
            //RoomsViewModel roomViewModel = new RoomsViewModel();
            //List<Room> roomList = new List<Room>()
            //{
            //    new Room {
            //        Id =  1,
            //        Name = "Studio 1",
            //        Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
            //        Image = "Studio1.jpg",
            //        Size = "800 * 750",
            //        Style= "K-pop;Hip Hop;Jazz",
            //        Rate = 100.00M
            //      },
            //      new Room {
            //        Id =  2,
            //        Name = "Studio 2",
            //        Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
            //        Image = "Studio2.jpg",
            //        Size = "800 * 750",
            //        Style= "K-pop;Hip Hop;Jazz",
            //        Rate = 120.00M
            //      },
            //      new Room {
            //        Id =  3,
            //        Name = "Studio 3",
            //        Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
            //        Image = "Studio3.jpg",
            //        Size = "800 * 750",
            //        Style= "K-pop;Hip Hop;Jazz",
            //        Rate = 100.00M
            //      },
            //      new Room {
            //        Id =  4,
            //        Name = "Studio 4",
            //        Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
            //        Image = "Studio4.jpg",
            //        Size = "800 * 750",
            //        Style= "K-pop;Hip Hop;Jazz",
            //        Rate = 100.00M
            //      },
            //      new Room {
            //        Id =  5,
            //        Name = "Studio 5",
            //        Description = "Measuring over 800 sqft and can accommodate up to 16 dancers comfortably. Our dance floor rests on a system of high-density rubber underlays and laminate wood flooring. It is fully air-conditioned and equipped with barres, mirrors and sound system compatible for audio input jack, USB device and SD card. This studio overlooks the street with many windows for natural lighting and ventilation.",
            //        Image = "Studio5.jpg",
            //        Size = "800 * 750",
            //        Style= "K-pop;Hip Hop;Jazz",
            //        Rate = 100.00M
            //      }
            //};
            //roomViewModel.Rooms = roomList;
            //roomViewModel.Error = 0;

            var result = _miscellaneousSerice.GetAllRoomType();

            if (result.Error != 0)
            {
                ViewBag.ErrorCode = result.Error.ToString();
                return View("Error");
            }
            return View(result);
        }

        public ActionResult CreateGet()
        {
            Room model = new Room();

            List<SelectListItem> styleOption = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text = "Hip Hop", Value = "Hip Hop"},
                    new SelectListItem(){ Text = "Jazz", Value = "Jazz"},
                    new SelectListItem(){ Text = "K-pop", Value = "K-pop"},
                    new SelectListItem(){ Text = "Latin", Value = "Latin"},
                    new SelectListItem(){ Text = "Breaking", Value = "Breaking"}
                };
            ViewBag.Style = new MultiSelectList(styleOption, "Value", "Text");


            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Room room, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> styleOption = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text = "Hip Hop", Value = "Hip Hop"},
                    new SelectListItem(){ Text = "Jazz", Value = "Jazz"},
                    new SelectListItem(){ Text = "K-pop", Value = "K-pop"},
                    new SelectListItem(){ Text = "Latin", Value = "Latin"},
                    new SelectListItem(){ Text = "Breaking", Value = "Breaking"}
                };
                ViewBag.Style = new MultiSelectList(styleOption, "Value", "Text");
                return View(room);
            }

            if (file != null && file.ContentLength > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                string[] allowedExtensions = { ".pdf", ".jpg", ".png",".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    
                    ViewBag.errorMessage = "Only PDF, JPG, PNG and GIF file extension supported.";
                    return View("Error");
                }

                //Store to ~/App_Data/Uploads folder
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/UserUploads"), fileName);
                file.SaveAs(path);

                //insert to db
                room.Image = fileName;
                room.Style = (room.StyleArr == null) ? "" : string.Join(",", room.StyleArr);
                room.CreateBy = string.Empty;
                room.CreatedDate = DateTime.Now;

                var result = _miscellaneousSerice.CreateRoomType(room);

                if (result != 0)
                {
                    System.IO.File.Delete(path);

                    ViewBag.ErrorCode = result.ToString();
                    return View("Error");
                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.errorMessage = "Something wrong with uploaded file.";
                return View("Error");
            }
        }

        public ActionResult ConfirmDelete(int roomId = 0)
        {
            //var result = _reservationService.DeleteTimeSlot(long.Parse(recordId));

            var result = _miscellaneousSerice.DeleteRoomType(roomId);

            if (result != 0)
            {
                ViewBag.ErrorCode = result.ToString();
                return View("Error");
            }

            return RedirectToAction("Index"); // 0 is success , other code is fail
        }

        public ActionResult EditGet(int roomId = 0)
        {
            var result = _miscellaneousSerice.FindRoomDetail(roomId);
          
            if (result.Error != 0)
            {
                ViewBag.errorMessage = "Room detail is not available.";
                return View("Error");
            }

            List<SelectListItem> styleOption = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text = "Hip Hop", Value = "Hip Hop"},
                    new SelectListItem(){ Text = "Jazz", Value = "Jazz"},
                    new SelectListItem(){ Text = "K-pop", Value = "K-pop"},
                    new SelectListItem(){ Text = "Latin", Value = "Latin"},
                    new SelectListItem(){ Text = "Breaking", Value = "Breaking"}
                };

            ViewBag.Style = new MultiSelectList(styleOption, "Value", "Text");

            result.Room.StyleArr = result.Room.Style.Split(',');
            result.Room.RateOri = result.Room.Rate;

            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(Room room,HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                string[] allowedExtensions = { ".pdf", ".jpg", ".png", ".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {

                    ViewBag.errorMessage = "Only PDF, JPG, PNG and GIF file extension supported.";
                    return View("Error");
                }

                //Store to ~/App_Data/Uploads folder
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/UserUploads"), fileName);
                file.SaveAs(path);
                
                //insert to db
                room.Image = fileName;
                room.Style = (room.StyleArr == null) ? "" : string.Join(",", room.StyleArr);
                room.UpdatedDate = DateTime.Now;
                room.UpdateBy = string.Empty;

                var result = _miscellaneousSerice.EditRoomType(room);

                if (result != 0)
                {
                    System.IO.File.Delete(path);

                    ViewBag.ErrorCode = result.ToString();
                    return View("Error");
                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.errorMessage = "Something wrong with uploaded file.";
                return View("Error");
            }
        }
    }
}