using StudioMember.Service.Contract;
using StudioMember.Service.Proxy;
using StudioMiscellaneous.Service.Contract;
using StudioMiscellaneous.Service.Proxy;
using StudioReservation.Contract;
using StudioReservation.Proxy;
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
        public static IReservationService _reservationService;
        public static IStudioMiscellaneousService MiscellaneousSevice;
        static ServiceConnection()
        {
            _memberService = new MemberServiceProxy(int.Parse(ConfigurationManager.AppSettings["member-service-port"]));

            _reservationService = new ReservationServiceProxy(int.Parse(ConfigurationManager.AppSettings["reservation-service-port"]));

            MiscellaneousSevice = new StudioMiscellaneousProxy(int.Parse(ConfigurationManager.AppSettings["miscellaneous-service-port"]));
        }

    }
}