using NLog;
using Redis;
using StudioMember.Service.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudioMember.Service.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var port = int.Parse(ConfigurationManager.AppSettings["port"]);

            Console.Title = $"Member Service-{port}";
            Console.WriteLine("[{0}] Member Service starting...", DateTime.Now);

            var url = $"net.tcp://localhost:{port}/MemberService";

            var repo = new MemberSQL(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            var redisIp = ConfigurationManager.AppSettings["redis-ip"];
            var redisPassword = ConfigurationManager.AppSettings["redis-password"];

            var RedisConnection = new RedisConnection(redisIp, redisPassword);

            var service = new MemberService(
                getAll: repo.GetAll,
                getAllRole : repo.GetMemberRole,
                update: repo.Update,
                updateDisable : repo.UpdateStatus,
                updateSubInfo : repo.UpdateRegisterTime,
                redis : RedisConnection
                );

            new ServiceHost<IMemberService>().Boot(url, service);

            Console.WriteLine($"{DateTime.Now} || Service started...");

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();

            Environment.Exit(0);
        }
        public static void Boot()
        {
        }

    }

    public class ServiceHost<T>
    {
        public void Boot(string url, T logicService)
        {
            Console.WriteLine("[{0}] booting with url {1}", DateTime.Now, url);
            var baseAddress = new Uri(url);

            var selfHost = new ServiceHost(logicService, baseAddress);
            var netTcpBinding = new NetTcpBinding { Security = { Mode = SecurityMode.None } };
            ServiceThrottlingBehavior item = new ServiceThrottlingBehavior
            {
                MaxConcurrentCalls = 100000
            };

            selfHost.Description.Behaviors.Add(item);
            selfHost.AddServiceEndpoint(typeof(T), netTcpBinding, url);
            selfHost.Faulted += SelfHost_Faulted;
            selfHost.Opened += SelfHost_Opened;
            selfHost.Open();
        }

        void SelfHost_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("[{0}] Service Openned", DateTime.Now);
        }

        void SelfHost_Faulted(object sender, EventArgs e)
        {
        }
    }

}
