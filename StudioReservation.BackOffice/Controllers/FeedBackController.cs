using Microsoft.AspNet.Identity;
using StudioFeedBack.DataModel;
using StudioMiscellaneous.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace StudioReservation.BackOffice.Controllers
{
    public class FeedBackController : Controller
    {
        IStudioMiscellaneousService _miscellaneousService;

        public FeedBackController()
        {
            _miscellaneousService = ServiceConnection.MiscellaneousSevice;
        }
        // GET: FeedBack
        public ActionResult Index(int status = 0)
        {
            var result = _miscellaneousService.GetAllByStatus(status);

            if (result.Error != 0)
            {
                ViewBag.ErrorCode = result.Error.ToString();
                return View("Error");
            }

            List<SelectListItem> statusFilter = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="All", Value="0", Selected = true},
                new SelectListItem(){ Text="Open", Value="1"},
                new SelectListItem(){ Text="Pending", Value="2"},
                new SelectListItem(){ Text="Closed", Value="3"},
            };
            ViewBag.Status = new SelectList(statusFilter, "Value", "Text");
            return View(result);
        }

        [HttpGet]
        public ActionResult GetDetail(long Id)
        {
            //var id = Guid.NewGuid().ToString();

            var result =_miscellaneousService.ViewDetail(Id);

            if (result.Error != 0)
            {
                ViewBag.ErrorCode = result.Error.ToString();
                return View("Error");
            }
            return View(result.FeedBacks);
            }

        [HttpPost]
        public ActionResult Reply(Feedback modal)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            modal.Message = modal.Comment;
            modal.Type = (int)FeedBackType.Admin;
            modal.ReplyName = User.Identity.GetUserName();
            modal.Status = (int)FeedbackStatus.Pending;

            //var result = _miscellaneousService.ReplyFeedback()

            return RedirectToAction("GetDetail", new { Id = modal.Id });
        }

        public ActionResult CloseTicket(String Id)
        {
            if (String.IsNullOrEmpty(Id))
                return View("Error");
            //var result = _miscellaneousService.ReplyFeedback()
            //if (result.Error != 0)
            //{
            //    ViewBag.ErrorCode = result.Error.ToString();
            //    return View("Error");
            //
            return RedirectToAction("Index");
        }


    }
}