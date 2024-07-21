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
        public ActionResult ViewAll()
        {

            var members = _memberService.GetAll();

            return View(members);
        }
    }
}