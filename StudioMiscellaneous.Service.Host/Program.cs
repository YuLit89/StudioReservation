using StackExchange.Redis;
using StudioMiscellaneous.Service.Contract;
using StudioRoomType.ADO;
using StudioRoomType.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Redis;
using StudioFeedBack.ADO;

namespace StudioMiscellaneous.Service.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var port = int.Parse(ConfigurationManager.AppSettings["port"]);

            Console.Title = $"MiscellaneousService Service-{port}";
            Console.WriteLine("[{0}] MiscellaneousService Service starting...", DateTime.Now);

            var url = $"net.tcp://localhost:{port}/MiscellaneousService";

            var roomRepo = new StudioRoomTypeSQL(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var feedbackRepo = new FeedbackSQL(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            var redisIp = ConfigurationManager.AppSettings["redis-ip"];
            var redisPassword = ConfigurationManager.AppSettings["redis-password"];

            var redis = new RedisConnection(redisIp, redisPassword);

            var feedbackService = new FeedbackHandler(
                getAll : feedbackRepo.GetAll,
                insert : feedbackRepo.Insert);

            var service = new MiscellaneousService(
                redisConnection : redis,
                getAllRoomType: roomRepo.GetAll,
                createRoomType: roomRepo.Create,
                updateRoomType: roomRepo.Edit,
                deleteRoomType: roomRepo.Delete,
                getAllUserFeedback : feedbackService.GetAll,
                submitFeedback : feedbackService.SubmitFeedback,
                replyFeedback : feedbackService.ReplyFeedback,
                findFeedbackDetail : feedbackService.ViewDetail
                );

            new ServiceHost<IStudioMiscellaneousService>().Boot(url, service);

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
            var netTcpBinding = new System.ServiceModel.NetTcpBinding { Security = { Mode = SecurityMode.None } };
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
