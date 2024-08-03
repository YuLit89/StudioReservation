
using NLog;
using StudioMember.DataModel;
using StudioMember.Service.Contract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        Func<string, bool, int> _updateDisable;
        Dictionary<string, Member> _members = new Dictionary<string, Member>();

        ConcurrentDictionary<string, object> _sync = new ConcurrentDictionary<string, object>();

        public MemberService(
            Func<IEnumerable<Member>> getAll,
            Func<string, string, string, int> update,
            Func<string,bool,int> updateDisable)
        {
            _getAll = getAll;
            _update = update;
            _updateDisable = updateDisable;

            foreach(var m in _getAll())
            {
                _members[m.Id] = m;
            }

            Console.WriteLine($"{DateTime.Now} || Pump data into local storage done...");
        }

        public void Dispose()
        {
        }

        public MemberViewModel Find(string Id)
        {
            var member = _sync.GetOrAdd(Id, new object());

            lock (member)
            {
                Member m;

                if(_members.TryGetValue(Id, out m))
                {
                    return new MemberViewModel
                    {
                        Member = m,
                        Error = 0
                    };
                }

                return new MemberViewModel
                {
                    Error = -10
                };
            }
        }

        public MemberViewModel FindByUserName(string Username)
        {
            var member = _members.Values.Where(x => x.UserName == Username)?.First();

            return (member != null) ? new MemberViewModel
            {
                Member = member,
                Error = 0
            } : new MemberViewModel
            {
                Error = -10
            };

        }

        public MembersViewModel GetAll()
        {
            return new MembersViewModel
            {
                Members = _members.Values.ToList(),
                Error = 0
            };
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

        public int SyncRegister(Member member)
        {
            _members.Add(member.Id, member);

            return 0;
        }


        public int UpdateUser(string MemberId, string Password, bool EmailConfirmed)
        {
            Member member = null;
            if(_members.TryGetValue(MemberId,out member))
            {
                member.Password = Password;
                member.EmailConfirmed = EmailConfirmed;

                _members[MemberId] = member;

                return 0;
            }

            return -10;
        }

        public int UpdateMemberStatus(string MemberId, bool isDisable)
        {
            Member member;
             if(_members.TryGetValue(MemberId,out member))
            {
                if (member.Role != "Admin") return -11;

                var result = _updateDisable(MemberId, isDisable);

                if(result == 0)
                {
                    member.isDisable = isDisable;

                    return 0;
                }

                return -10;
            }

            return -12;
        }
    }
}
