using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            return View(result.Room);
        }
        [HttpPost]
        public ActionResult Edit(Room room,HttpPostedFileBase file)
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
                room.RateOri = room.Rate;
                return View(room);
            }

            string fileName = "";
            string path = "";
            bool newupload = false;
            if (file != null && file.ContentLength > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                string[] allowedExtensions = { ".pdf", ".jpg", ".png", ".gif" };
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ViewBag.errorMessage = "Only PDF, JPG, PNG and GIF file extension supported.";
                    return View("Error");
                }
                //Store to ~/Images/Uploads folder
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff")+ fileExtension;
                path = Path.Combine(Server.MapPath("~/Images/UserUploads"), fileName);
                file.SaveAs(path);
                newupload = true;
            }
            else if (file == null && !string.IsNullOrEmpty(room.Image))
            {
                fileName = room.Image;
            }
            else
            {
                return View("Error");
            }         
                //insert to db
                room.Image = fileName;
                room.Style = (room.StyleArr == null) ? "" : string.Join(",", room.StyleArr);
                room.UpdatedDate = DateTime.Now;
                room.UpdateBy = string.Empty;

                var result = _miscellaneousSerice.EditRoomType(room);

                if (result != 0)
                {
                    if(newupload)
                        System.IO.File.Delete(path);

                    ViewBag.ErrorCode = result.ToString();
                    return View("Error");
                }

                return RedirectToAction("Index");
        }     
    }
}