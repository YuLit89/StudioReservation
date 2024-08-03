using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioMember.DataModel
{
    public class Member
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
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
