using Redis;
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
        public static IMemberService MemberService;
        public static IRedisConnection RedisConnection;

        static ServiceConnection()
        {

            var redisIp = ConfigurationManager.AppSettings["redis-ip"];
            var redisPassword = ConfigurationManager.AppSettings["redis-password"];

            RedisConnection = new RedisConnection(redisIp,redisPassword);
            MemberService = new MemberServiceProxy(int.Parse(ConfigurationManager.AppSettings["member-service-port"]));
            ReservationService = new ReservationServiceProxy(int.Parse(ConfigurationManager.AppSettings["reservation-service-port"]));
        }

    }
}