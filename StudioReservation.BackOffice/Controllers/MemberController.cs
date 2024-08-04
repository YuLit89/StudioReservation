using StudioMember.DataModel;
using StudioMember.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudioReservation.BackOffice.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Web.UI.WebControls;
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
            Member model = new Member();
            List<SelectListItem> countryCodeList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="60", Value="60", Selected = true},
                new SelectListItem(){ Text="65", Value="65"}
            };            
            ViewBag.CountryCode = new SelectList(countryCodeList, "Value", "Text");
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Member member)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> countryCodeList = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text="60", Value="60" ,Selected = true},
                    new SelectListItem(){ Text="65", Value="65"}
                };
                ViewBag.CountryCode = new SelectList(countryCodeList, "Value", "Text");

                return View(member);
            }
            //store to db
            member.PhoneNumber = member.CountryCode + member.PhoneNumber;

            var user = new ApplicationUser { UserName = member.UserName, Email = member.Email, PhoneNumber = member.PhoneNumber };
            var result = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().CreateAsync(user, member.Password);

            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().AddToRoleAsync(user.Id, "Admin");

            _memberService.UpdateRegisterSubInfo(user.Id, DateTime.Now, "");

            if (!result.Result.Succeeded)
            {
                ViewBag.errorMessage = string.Join(",", result.Result.Errors);
                return View("Error");
            }
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult EditGet(string memberId)
        {
            var result = _memberService.Find(memberId);

            if(result.Error != 0)
            {
                ViewBag.ErrorCode = result.Error.ToString();
                return View("Error");
            }
            result.Member.ConfirmPassword = result.Member.Password;
            return View(result.Member);
        }

        [HttpPost]
        public ActionResult Edit(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View(member);
            }
            //updating db
            var result = _memberService.Update(member.Id, member.UserName, member.PhoneNumber, DateTime.Now,false);

            if (result != 0)
            {
                ViewBag.ErrorCode = result.ToString();
                return View("Error");
            }

            return RedirectToAction("Index");


        }

    }
}