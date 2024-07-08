using StudioMember.Service.Contract;
using StudioMember.Service.Proxy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace StudioReservation.BackOffice
{
   
    public static class ServiceConnection
    {
        public static IMemberService _memberService;

        static ServiceConnection()
        {
            _memberService = new MemberServiceProxy(int.Parse(ConfigurationManager.AppSettings["member-service-port"]));
        }

    }
}