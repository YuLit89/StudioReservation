using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudioMember.DataModel
{
    public class Member
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^\\d+", ErrorMessage = "Incorrect Phone Number format.")]
        [Display(Name = "Phone Number*")]
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        [Required]
        [Display(Name = "User Name*")]
        public string UserName { get; set; }
        public string Ip { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Role { get; set; }
        public bool isDisable {  get; set; }
    }

    public class MembersViewModel
    {
        public List<Member> Members { get; set; }
        public int Error { get; set; }
    }

    public class MemberViewModel
    {
        public Member Member { get; set; }
        public int Error { get; set; }
    }
}
