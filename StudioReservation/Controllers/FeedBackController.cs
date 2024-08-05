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
            return View();
        }

        public ActionResult SubmitFeedBack(SubmitFeedBack submit)
        {

            var result = new SubmitFeedBackResponse
            {
                Error = 0,
                TicketId = Guid.NewGuid().ToString()
            };

            return View(result);
        }
    }
}