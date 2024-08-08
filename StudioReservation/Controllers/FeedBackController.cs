using StudioFeedBack.DataModel;
using StudioMiscellaneous.Service;
using StudioMiscellaneous.Service.Contract;
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
        IStudioMiscellaneousService _miscellaneousService;

        public FeedBackController()
        {
            _miscellaneousService = ServiceConnection.MiscellaneousService;
        }

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

            var result = _miscellaneousService.SubmitFeedback(submit, DateTime.Now);

            return RedirectToAction("FeedbackAck",result);
        }

        public ActionResult FeedbackAck(SubmitFeedbackResponse modal)
        {            
            return View(modal);
        }
    }
}