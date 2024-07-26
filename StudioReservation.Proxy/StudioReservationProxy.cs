using NLog;
using StudioReservation.Contract;
using StudioReservation.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudioReservation.Proxy
{
    public class ReservationServiceProxy : IReservationService
    {
        readonly EndpointAddress _endpoint;
        readonly ChannelFactory<IReservationService> _channelFactory;

        public ReservationServiceProxy(int port)
        {
            var url = $"net.tcp://localhost:{port}/ReservationService";

            var netTcpBinding = new NetTcpBinding { Security = { Mode = SecurityMode.None } };

            _endpoint = new EndpointAddress(url);
            _channelFactory = new ChannelFactory<IReservationService>(netTcpBinding);

        }

        public int CreateTimeSlot(CreateRoomTimeSlot Request)
        {
            IReservationService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.CreateTimeSlot(Request);
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

        public int EditTimeSlot(long TimeSlotId, string Times, string UpdateBy, DateTime UpdateTime, bool Enable)
        {
            IReservationService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.EditTimeSlot(TimeSlotId, Times, UpdateBy, UpdateTime, Enable);
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

        public ViewAllTimeSlot FindAllRoomTimeSlot(int Page, long LastId, int Size = 20)
        {
            IReservationService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.FindAllRoomTimeSlot(Page,LastId,Size);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return new ViewAllTimeSlot { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return new ViewAllTimeSlot { Error = -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return new ViewAllTimeSlot { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return new ViewAllTimeSlot { Error = -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return new ViewAllTimeSlot { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public ViewAllTimeSlot FindRoomTimeSlotByFilter(int RoomId, DateTime StartTime, DateTime EndTime, int Page, long LastId, int Size = 20)
        {
            IReservationService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.FindRoomTimeSlotByFilter(RoomId,StartTime,EndTime,Page,LastId,Size);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return new ViewAllTimeSlot { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return new ViewAllTimeSlot { Error = -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return new ViewAllTimeSlot { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return new ViewAllTimeSlot { Error = -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return new ViewAllTimeSlot { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public int TimeSlotReservation(TimeSlotReservationRequest Request)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
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

        public RoomTimeSlotDetail FindDetail(long TimeSlotId)
        {
            IReservationService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.FindDetail(TimeSlotId);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return new RoomTimeSlotDetail { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return new RoomTimeSlotDetail { Error = -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return new RoomTimeSlotDetail { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return new RoomTimeSlotDetail { Error = -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return new RoomTimeSlotDetail { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }
    }
}
