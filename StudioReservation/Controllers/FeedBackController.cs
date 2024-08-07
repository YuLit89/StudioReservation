using StudioFeedBack.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudioReservation.Controllers
{
    public class FeedBackController : Controller
    {
        // GET: FeedBack
        public ActionResult Index()
        {
            SubmitFeedback model = new SubmitFeedback();
            model.Preference = "Email";
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitFeedBack(SubmitFeedback submit)
        {
            if (!ModelState.IsValid)
            {
                return View(submit);
            }

            var result = new SubmitFeedbackResponse()
            {
                TicketId = "0120012",
                Error = 0
            };

            return RedirectToAction("FeedbackAck",result);
        }

        public ActionResult FeedbackAck(SubmitFeedbackResponse modal)
        {            
            return View(modal);
        }
    }
}