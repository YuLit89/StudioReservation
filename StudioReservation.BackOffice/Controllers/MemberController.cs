using StudioMember.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            return View(result);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            var result = _memberService.Find(id);

            return View(result);
        }

    }
}