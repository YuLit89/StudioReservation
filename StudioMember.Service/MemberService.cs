using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using StudioMember.Service.Contract;
using StudioMember.Service.Contract.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudioMember.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class MemberService : IMemberService
    {
        Func<IEnumerable<Member>> _getAll;
        Func<string, string, string, int> _update;

        Dictionary<string, Member> _members = new Dictionary<string, Member>();

        ConcurrentDictionary<string, object> _sync = new ConcurrentDictionary<string, object>();

        public MemberService(
            Func<IEnumerable<Member>> getAll,
            Func<string, string, string, int> update)
        {
            _getAll = getAll;
            _update = update;

            foreach(var m in _getAll())
            {
                _members[m.Id] = m;
            }

            Console.WriteLine($"{DateTime.Now} || Pump data into local storage done...");
        }

        public Tuple<int, IdentityResult> ConfirmEmail(string userId, string code)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Member Find(string Id)
        {
            var member = _sync.GetOrAdd(Id, new object());

            lock (member)
            {
                Member m;
                return _members.TryGetValue(Id, out m) ? m : null;
            }
        }

        public Member FindByUserName(string Username)
        {
            var member = _members.Values.Where(x => x.UserName == Username)?.First();

            return (member == null) ? null : member;

        }

        public List<Member> GetAll()
        {
            return _members.Values.ToList();
        }

        public int Update(string Id,string Email, string PhoneNumber)
        {
            var member = _sync.GetOrAdd(Id, new object());

            lock (member)
            {
                var err = _update(Id, Email, PhoneNumber);

                if(err == 0)
                {
                    var m = _members[Id];
                    
                    m.Email = Email;
                    m.PhoneNumber = PhoneNumber;

                    return 0;
                }

                LogManager.GetCurrentClassLogger().Error($"Update Member fail id :{Id}");
                return -10;
            }
        }

        public Tuple<int, SignInStatus> Login(string Email, string Password, bool RememberMe)
        {
            throw new NotImplementedException();
        }

        public int ForgotPassword(string Email)
        {
            throw new NotImplementedException();
        }
      

        public Tuple<int, IdentityResult> Register(string Email, string Password, string ConfirmPassword, string ContactNumber, string FullName)
        {
            throw new NotImplementedException();
        }

        public Tuple<int, IdentityResult> ResetPassword(string Email, string Password, string ConfirmPassword, string Code)
        {
            throw new NotImplementedException();
        }

    }
}
