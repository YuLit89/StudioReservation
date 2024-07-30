using StudioMember.Service.Contract;
using StudioMember.Service.Proxy;
using StudioReservation.Contract;
using StudioReservation.Proxy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace StudioReservation
{
    public static class ServiceConnection
    {
        public static IReservationService ReservationService;

        static ServiceConnection()
        {

            ReservationService = new ReservationServiceProxy(int.Parse(ConfigurationManager.AppSettings["reservation-service-port"]));
        }

    }
}