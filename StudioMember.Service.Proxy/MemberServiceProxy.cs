
using NLog;
using StudioMember.DataModel;
using StudioMember.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudioMember.Service.Proxy
{
    public class MemberServiceProxy : IMemberService
    {
        readonly EndpointAddress _endpoint;
        readonly ChannelFactory<IMemberService> _channelFactory;

        public MemberServiceProxy(int port)
        {
            var url = $"net.tcp://localhost:{port}/MemberService";

            var netTcpBinding = new NetTcpBinding { Security = { Mode = SecurityMode.None } };

            _endpoint = new EndpointAddress(url);
            _channelFactory = new ChannelFactory<IMemberService>(netTcpBinding);

        }

        public void Dispose()
        {
          
        }

        public MemberViewModel Find(string Id)
        {
            IMemberService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.Find(Id);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1 , Request {Id}");
                return new MemberViewModel { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2 , Request {Id}");
                return new MemberViewModel { Error = -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3 , Request {Id}");
                return new MemberViewModel { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4 , Request {Id}");
                return new MemberViewModel { Error = -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5 , Request {Id}");
                return new MemberViewModel { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public MemberViewModel FindByUserName(string Username)
        {
            IMemberService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.Find(Username);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1 , Request {Username}");
                return new MemberViewModel { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2 , Request {Username}");
                return new MemberViewModel { Error = -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3 , Request {Username}");
                return new MemberViewModel { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4 , Request {Username}");
                return new MemberViewModel { Error = -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5 , Request {Username}");
                return new MemberViewModel { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public MembersViewModel GetAll()
        {
            IMemberService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.GetAll();
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1");
                return new MembersViewModel { Error = -1 };
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2");
                return new MembersViewModel { Error = -2 };
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3");
                return new MembersViewModel { Error = -3 };
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4");
                return new MembersViewModel { Error = -4 };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5");
                return new MembersViewModel { Error = -5 };
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public int Update(string Id, string Email, string PhoneNumber)
        {
            IMemberService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.Update(Id,Email,PhoneNumber);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1 , Request {Id},{Email},{PhoneNumber}");
                return -1;
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2 , Request {Id},{Email},{PhoneNumber}");
                return -2;
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3 , Request {Id},{Email},{PhoneNumber}");
                return -3;
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4 , Request {Id},{Email},{PhoneNumber}");
                return -4;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5 , Request {Id},{Email},{PhoneNumber}");
                return -5;
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

        public int Register(string MemberId, string Email, bool EmailConfirmed, string Password, string PhoneNumber, string UserName)
        {
            IMemberService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.Register(MemberId,Email,EmailConfirmed,Password,PhoneNumber,UserName);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1 , Request {MemberId}");
                return -1;
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2 , Request {MemberId}");
                return -2;
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3 , Request {MemberId}");
                return -3;
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4 , Request {MemberId}");
                return -4;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5 , Request {MemberId}");
                return -5;
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }

        public int UpdateUser(string MemberId, string Password, bool EmailConfirmed)
        {
            IMemberService s = null;
            try
            {
                s = _channelFactory.CreateChannel(_endpoint);

                if (s != null)
                {
                    return s.UpdateUser(MemberId,Password,EmailConfirmed);
                }

                LogManager.GetCurrentClassLogger().Error($"Proxy Error -1 , Request {MemberId}");
                return -1;
            }
            catch (FaultException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -2 , Request {MemberId}");
                return -2;
            }
            catch (CommunicationException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -3 , Request {MemberId}");
                return -3;
            }
            catch (TimeoutException ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -4 , Request {MemberId}");
                return -4;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error($"Proxy Error -5 , Request {MemberId}");
                return -5;
            }
            finally
            {
                CloseOrAbortServiceChannel((ICommunicationObject)s);
            }
        }
    }
}
