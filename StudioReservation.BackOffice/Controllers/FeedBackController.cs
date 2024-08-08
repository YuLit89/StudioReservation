﻿using StudioFeedBack.DataModel;
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

            var result = _miscellaneousService.GetAll();

            if (result.Error != 0)
            {
                ViewBag.ErrorCode = result.Error.ToString();
                return View("Error");
            }
            return View(result);

            //var result = new FeedbackViewModel // only return type = 1
            //{
            //    Error = 0 ,
            //    FeedBacks = new List<Feedback>
            //    {
            //        new Feedback
            //        {
            //            Id = 1,
            //            TicketId = Guid.NewGuid().ToString(),
            //            Title = "Payment issue",
            //            UserEmail = "",
            //            UserPhoneNumber = "60-177272973",
            //            Message = "payment already complete but history not found",
            //            SubmitTime = DateTime.Now,
            //            Type = 1,
            //            Preference = "Call",
            //            IsCompleted = false
            //        },
            //           new Feedback
            //        {
            //            Id = 2,
            //            TicketId = Guid.NewGuid().ToString(),
            //            Title = "account issue",
            //            UserEmail = "",
            //            UserPhoneNumber = "60-177272973",
            //            Message = "payment already complete but history not found",
            //            SubmitTime = DateTime.Now,
            //            Type = 1,
            //            Preference = "Call",
            //            IsCompleted = false,
            //        }
            //    }

            //};

            return View();
        }

        [HttpGet]
        public ActionResult GetDetail(long Id)
        {
            var id = Guid.NewGuid().ToString();

            var result =_miscellaneousService.ViewDetail(Id);
            //var result = new FeedbackViewModel //
            //{
            //    Error = 0,
            //    FeedBacks = new List<Feedback>
            //    {
            //        new Feedback
            //        {
            //            Id = 1,
            //            TicketId = id,
            //            Title = "Payment issue",
            //            UserEmail = "",
            //            UserPhoneNumber = "60-177272973",
            //            Message = "payment already complete but history not found",
            //            SubmitTime = DateTime.Now,
            //            Type = 1,
            //            Preference = "Call",
            //            IsCompleted = false
            //        },
            //           new Feedback
            //        {
            //            Id = 2,
            //            TicketId = id,
            //            Title = "",
            //            UserEmail = "",
            //            UserPhoneNumber = "",
            //            Message = "issue have been fixed , please refresh again",
            //            SubmitTime = DateTime.Now,
            //            Type = 2,
            //            Preference = "",
            //            IsReplyed = true,
            //            IsCompleted = false,
            //        },
            //                new Feedback
            //        {
            //            Id = 3,
            //            TicketId = id,
            //            Title = "",
            //            UserEmail = "",
            //            UserPhoneNumber = "",
            //            Message = "OK , thank for help",
            //            SubmitTime = DateTime.Now,
            //            Type = 1,
            //            Preference = "",
            //            IsReplyed = true,
            //            IsCompleted = false,
            //        },
            //        new Feedback
            //        {
            //            Id = 4,
            //            TicketId = id,
            //            Title = "",
            //            UserEmail = "",
            //            UserPhoneNumber = "",
            //            Message = "Welcome , have a nice day ",
            //            SubmitTime = DateTime.Now,
            //            Type = 2,
            //            Preference = "",
            //            IsReplyed = true,
            //            IsCompleted = true,
            //        }
            //    }
            //};

                return View(result);
            }

        [HttpPost]
        public ActionResult Reply()
        {

          //var result = _miscellaneousService.ReplyFeedback(string.Empty);

           return View();
        }


    }
}