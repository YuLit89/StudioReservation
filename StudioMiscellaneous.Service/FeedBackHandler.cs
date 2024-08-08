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

        SubmitFeedbackResponse SubmitFeedback(SubmitFeedback Feedback, DateTime SubmitTime);

        int ReplyFeedback(string AdminId ,string TicketId, string Message ,DateTime SubmitTime,bool isComplete);

        FeedbackViewModel ViewDetail(long Id);

    }


    public class FeedbackHandler : IFeedbackHandler
    {
        public void Dispose()
        {
        }

        Func<Feedback, long> _insert;

        Dictionary<long,Feedback> _feedbacks = new Dictionary<long,Feedback>();
        Dictionary<string,HashSet<long>> _ticketIds = new Dictionary<string,HashSet<long>>();
        
        public FeedbackHandler(
            Func<IEnumerable<Feedback>> getAll,
            Func<Feedback,long> insert)
        {

            _insert = insert;

            foreach(var i in getAll())
            {
                _feedbacks[i.Id] = i;

                HashSet<long> ids;
                if(_ticketIds.TryGetValue(i.TicketId, out ids))
                {
                    ids.Add(i.Id);
                }

                _ticketIds[i.TicketId] = (ids == null) ? new HashSet<long> { i.Id } : ids; 
                
            }

            Console.WriteLine($"{DateTime.Now} || Pump Feedback DATA Done , record {_feedbacks.Count()}");
        }

        public FeedbackViewModel GetAll()
        {
            var userFeedbacks = _feedbacks.Where(x => x.Value.Type == 1)
                .Select(x1 => x1.Value).OrderBy(x2 => x2.SubmitTime).ToList();

            return new FeedbackViewModel
            {
                Error = 0,
                FeedBacks = userFeedbacks
            };
        }

        public int ReplyFeedback(string AdminId, string TicketId, string Message, DateTime SubmitTime, bool isComplete)
        {
            var reply = new Feedback
            {
                TicketId = TicketId,
                Title = string.Empty,
                UserEmail = string.Empty,
                UserPhoneNumber = string.Empty,
                Message = Message,
                SubmitTime = SubmitTime,
                Type = (int)FeedBackType.Admin,
                Preference = string.Empty,
                ReplyName = AdminId,
                Status = (int)FeedbackStatus.Pending
            };

            var id = _insert(reply);

            if (id <= 0) return -10;

            reply.Id = id;
            _feedbacks[id] = reply;

            HashSet<long> ids;
            if(_ticketIds.TryGetValue(TicketId,out ids))
            {
                ids.Add(id);
            }

            return 0;

        }

        public SubmitFeedbackResponse SubmitFeedback(SubmitFeedback Feedback,DateTime SubmitTime)
        {
            var feedback = new Feedback
            {
                TicketId = Guid.NewGuid().ToString(),
                Title = Feedback.Title,
                UserEmail = Feedback.UserEmail,
                UserPhoneNumber = Feedback.UserPhoneNumber,
                Message = Feedback.Message,
                SubmitTime = SubmitTime,
                Type = (int)FeedBackType.User,
                Preference = Feedback.Preference,
                ReplyName = string.Empty,
            };

            var id = _insert(feedback);

            if (id <= 0) return new SubmitFeedbackResponse { Error = -10 };

            feedback.Id = id;
            _feedbacks[id] = feedback;

            _ticketIds.Add(feedback.TicketId, new HashSet<long> { id });

            return new SubmitFeedbackResponse
            {
                Error = 0,
                TicketId = feedback.TicketId
            };
        }

        public FeedbackViewModel ViewDetail(long Id)
        {
              var feedbackDetail = new List<Feedback>();

              Feedback feedback;
              if(_feedbacks.TryGetValue(Id,out feedback))
              {
                    HashSet<long> ids;
                    if(_ticketIds.TryGetValue(feedback.TicketId,out ids))
                    {
                        foreach(var id in ids)
                        {
                           _feedbacks.TryGetValue(id, out feedback);

                           feedbackDetail.Add(feedback);     
                                                  
                        }

                        return new FeedbackViewModel
                        {
                            FeedBacks = feedbackDetail.OrderBy(x => x.SubmitTime).ToList(),
                            Error = 0
                        };
                    }
              }

            return new FeedbackViewModel
            {
                Error = -10
            };


        }
    }
}
