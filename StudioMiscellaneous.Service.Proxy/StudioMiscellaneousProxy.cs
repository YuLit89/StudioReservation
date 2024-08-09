using NLog;
using StudioFeedBack.DataModel;
using StudioMiscellaneous.Service.Contract;
using StudioRoomType.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudioMiscellaneous.Service.Proxy
{
    public class StudioMiscellaneousProxy : IStudioMiscellaneousService
    {

        readonly EndpointAddress _endpoint;
        readonly ChannelFactory<IStudioMiscellaneousService> _channelFactory;

        public StudioMiscellaneousProxy(int port)
        {
            var url = $"net.tcp://localhost:{port}/MiscellaneousService";

            var netTcpBinding = new NetTcpBinding { Security = { Mode = SecurityMode.None } };

            _endpoint = new EndpointAddress(url);
            _channelFactory = new ChannelFactory<IStudioMiscellaneousService>(netTcpBinding);

        }

        public int CreateRoomType(Room Room)
        {
            IStudioMiscellaneousService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.CreateRoomType(Room);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return -1;
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return -2;
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return -3;
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return -4;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return -5;
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public int DeleteRoomType(int RoomId)
        {
            IStudioMiscellaneousService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.DeleteRoomType(RoomId);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return -1;
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return -2;
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return -3;
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return -4;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return -5;
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public void Dispose()
        {
        }

        public int EditRoomType(Room room)
        {
            IStudioMiscellaneousService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.EditRoomType(room);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return -1;
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return -2;
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return -3;
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return -4;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return -5;
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public RoomsViewModel GetAllRoomType()
        {
            IStudioMiscellaneousService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.GetAllRoomType();
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return new RoomsViewModel { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return new RoomsViewModel { Error = -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return new RoomsViewModel { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return new RoomsViewModel { Error = -4};
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return new RoomsViewModel { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }
        void CloseOrAbortServiceChannel(ICommunicationObject communicationObject)
        {
            var isClosed = false;

            if (communicationObject == null || communicationObject.State == CommunicationState.Closed)
            {
                return;
            }

            try
            {
                if (communicationObject.State != CommunicationState.Faulted)
                {
                    communicationObject.Close();
                    isClosed = true;
                }
            }
            catch (CommunicationException)
            {
                // Catch this expected exception so it is not propagated further.
                // Perhaps write this exception out to log file for gathering statistics...
            }
            catch (TimeoutException)
            {
                // Catch this expected exception so it is not propagated further.
                // Perhaps write this exception out to log file for gathering statistics...
            }
            catch (Exception)
            {
                // An unexpected exception that we don't know how to handle.
                // Perhaps write this exception out to log file for support purposes...
                //throw;
            }
            finally
            {
                // If State was Faulted or any exception occurred while doing the Close(), then do an Abort()
                if (!isClosed)
                {
                    AbortServiceChannel(communicationObject);
                }
            }
        }

        void AbortServiceChannel(ICommunicationObject communicationObject)
        {
            try
            {
                communicationObject.Abort();
            }
            catch (Exception)
            {
                // An unexpected exception that we don't know how to handle.
                // If we are in this situation:
                // - we should NOT retry the Abort() because it has already failed and there is nothing to suggest it could be successful next time
                // - the abort may have partially succeeded
                // - the actual service call may have been successful
                //
                // The only thing we can do is hope that the channel's resources have been released.
                // Do not rethrow this exception because the actual service operation call might have succeeded
                // and an exception closing the channel should not stop the client doing whatever it does next.
                //
                // Perhaps write this exception out to log file for gathering statistics and support purposes...
            }
        }

        public RoomViewDetail FindRoomDetail(int RoomId)
        {
            IStudioMiscellaneousService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.FindRoomDetail(RoomId);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return new RoomViewDetail { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return new RoomViewDetail { Error = -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return new RoomViewDetail { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return new RoomViewDetail { Error = -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return new RoomViewDetail { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public FeedbackViewModel GetAllByStatus(int status)
        {
            IStudioMiscellaneousService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.GetAllByStatus(status);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return new FeedbackViewModel { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return new FeedbackViewModel { Error= -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return new FeedbackViewModel { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return new FeedbackViewModel { Error = -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return new FeedbackViewModel { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public SubmitFeedbackResponse SubmitFeedback(SubmitFeedback Feedback, DateTime SubmitTime)
        {
            IStudioMiscellaneousService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.SubmitFeedback(Feedback,SubmitTime);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return new SubmitFeedbackResponse { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return new SubmitFeedbackResponse { Error= -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return new SubmitFeedbackResponse { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return new SubmitFeedbackResponse { Error = -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return new SubmitFeedbackResponse { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public int ReplyFeedback(string AdminId, string TicketId, string Message, DateTime SubmitTime, bool isComplete)
        {
            IStudioMiscellaneousService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.ReplyFeedback(AdminId,TicketId,Message,SubmitTime,isComplete);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return -1;
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return -2;
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return -3;
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return -4;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return -5;
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public FeedbackViewModel ViewDetail(long Id)
        {
            IStudioMiscellaneousService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.ViewDetail(Id);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return new FeedbackViewModel { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return new FeedbackViewModel { Error = -2 };
            }   
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return new FeedbackViewModel { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return new FeedbackViewModel { Error= -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return new FeedbackViewModel { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }
    }
}
