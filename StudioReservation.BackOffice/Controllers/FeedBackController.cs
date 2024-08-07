using StudioFeedBack.DataModel;
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
        // GET: FeedBack
        public ActionResult Index()
        {
           
            var result = new FeedbackViewModel // only return type = 1
            {
                Error = 0 ,
                FeedBacks = new List<Feedback>
                {
                    new Feedback
                    {
                        Id = 1,
                        TicketId = Guid.NewGuid().ToString(),
                        Title = "Payment issue",
                        UserEmail = "",
                        UserPhoneNumber = "60-177272973",
                        Message = "payment already complete but history not found",
                        SubmitTime = DateTime.Now,
                        Type = 1,
                        Preference = "Call",
                        IsCompleted = false
                    },
                       new Feedback
                    {
                        Id = 2,
                        TicketId = Guid.NewGuid().ToString(),
                        Title = "account issue",
                        UserEmail = "",
                        UserPhoneNumber = "60-177272973",
                        Message = "payment already complete but history not found",
                        SubmitTime = DateTime.Now,
                        Type = 1,
                        Preference = "Call",
                        IsCompleted = false,
                    }
                }

            };

            return View();
        }

        [HttpGet]
        public ActionResult GetDetail(long Id)
        {
            var id = Guid.NewGuid().ToString();
            var result = new FeedbackViewModel //
            {
                Error = 0,
                FeedBacks = new List<Feedback>
                {
                    new Feedback
                    {
                        Id = 1,
                        TicketId = id,
                        Title = "Payment issue",
                        UserEmail = "",
                        UserPhoneNumber = "60-177272973",
                        Message = "payment already complete but history not found",
                        SubmitTime = DateTime.Now,
                        Type = 1,
                        Preference = "Call",
                        IsCompleted = false
                    },
                       new Feedback
                    {
                        Id = 2,
                        TicketId = id,
                        Title = "",
                        UserEmail = "",
                        UserPhoneNumber = "",
                        Message = "issue have been fixed , please refresh again",
                        SubmitTime = DateTime.Now,
                        Type = 2,
                        Preference = "",
                        IsReplyed = true,
                        IsCompleted = false,
                    },
                            new Feedback
                    {
                        Id = 3,
                        TicketId = id,
                        Title = "",
                        UserEmail = "",
                        UserPhoneNumber = "",
                        Message = "OK , thank for help",
                        SubmitTime = DateTime.Now,
                        Type = 1,
                        Preference = "",
                        IsReplyed = true,
                        IsCompleted = false,
                    },
                    new Feedback
                    {
                        Id = 4,
                        TicketId = id,
                        Title = "",
                        UserEmail = "",
                        UserPhoneNumber = "",
                        Message = "Welcome , have a nice day ",
                        SubmitTime = DateTime.Now,
                        Type = 2,
                        Preference = "",
                        IsReplyed = true,
                        IsCompleted = true,
                    }
                }
            };

                return View(result);
            }

        [HttpPost]
        public ActionResult Reply()
        {


           return View();
        }


    }
}