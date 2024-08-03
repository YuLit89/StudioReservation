using StudioMember.DataModel;
using StudioMember.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudioReservation.BackOffice.Models;
namespace StudioReservation.BackOffice.Controllers
{
    public class MemberController : Controller
    {
        IMemberService _memberService;
        public MemberController() 
        {
            _memberService = ServiceConnection._memberService;

        }

        // GET: Member
        [HttpGet]
        public ActionResult Index()
        {
            var result = _memberService.GetAll();

            if (result.Error != 0)
            {
                ViewBag.ErrorCode = result.Error.ToString();
                return View("Error");
            }
            return View(result);
        }
        [HttpGet]
        public ActionResult CreateGet()
        {
            MemberViewModel model = new MemberViewModel();

            return View(model);
        }


        [HttpGet]
        public ActionResult EditGet(string memberId)
        {
            return View();

        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            //var result = _memberService.Find(id);

            //return View(result);
            return View();
        }

    }
}