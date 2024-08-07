using StudioFeedBack.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioMiscellaneous.Service
{
    public interface IFeedbackHandler : IDisposable
    {

        FeedbackViewModel GetAll();

        SubmitFeedbackResponse SubmitFeedback(SubmitFeedback Feedback);

        int ReplyFeedback(string AdminId ,string TicketId, string Message ,DateTime SubmitTime,bool isComplete);

        FeedbackViewModel ViewDetail(long Id);

    }


    public class FeedbackHandler : IFeedbackHandler
    {
        public void Dispose()
        {
        }

        public FeedbackViewModel GetAll()
        {
            throw new NotImplementedException();
        }

        public int ReplyFeedback(string AdminId, string TicketId, string Message, DateTime SubmitTime, bool isComplete)
        {
            throw new NotImplementedException();
        }

        public SubmitFeedbackResponse SubmitFeedback(SubmitFeedback Feedback)
        {
            throw new NotImplementedException();
        }

        public FeedbackViewModel ViewDetail(long Id)
        {
            throw new NotImplementedException();
        }
    }
}
