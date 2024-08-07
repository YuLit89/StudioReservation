using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioFeedBack.DataModel
{
    public class Feedback //admin use
    { 
        public long Id { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber {  get; set; }
        public string Message {  get; set; }
        public DateTime SubmitTime { get; set; }
        public int Type { get; set; } // 1 = User  , 2 = Admin
        public string Preference { get; set; } // email / whatapp / wechat / line / call 
        public string ReplyName { get; set; } // store admin name when reply feedback else will empty
        public bool IsReplyed { get; set; }
        public bool IsCompleted { get; set; }

        public Feedback()
        {
            //TicketId = Guid.NewGuid().ToString();
        }
    }

    public class FeedbackViewModel // admin page use
    {
        public List<Feedback> FeedBacks { get; set; }
        public int Error { get; set; }
    }

    public class SubmitFeedback // user submit page use
    {
        public string Title { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public string Message { get; set; }
        public string Preference { get; set; } // email / whatapp / wechat / line / call 
    }

    public class SubmitFeedbackResponse 
    {
        public string TicketId { get; set; }
        public int Error { get; set; }
    }

}
