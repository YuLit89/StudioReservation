
using NLog;
using StudioMember.DataModel;
using StudioMember.Service.Contract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudioMember.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class MemberService : IMemberService
    {
        Func<IEnumerable<Member>> _getAll;
        Func<string, string, string,bool,DateTime, int> _update;

        Dictionary<string, Member> _members = new Dictionary<string, Member>();

        ConcurrentDictionary<string, object> _sync = new ConcurrentDictionary<string, object>();

        public MemberService(
            Func<IEnumerable<Member>> getAll,
            Func<string, string, string, bool,DateTime, int> update)
        {
            _getAll = getAll;
            _update = update;

            foreach (var m in _getAll())
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

      

        public int Update(string Id,string NickName, string PhoneNumber,DateTime updateTime, bool Disable = false)
        {
            var sync = _sync.GetOrAdd(Id, new object());

            lock (sync)
            {
                Member member;
                if(_members.TryGetValue(Id,out member))
                {
                    var result = _update(Id, NickName, PhoneNumber,Disable,updateTime);

                    if(result == 0)
                    {
                        member.UserName = NickName;
                        member.PhoneNumber = PhoneNumber;
                        member.isDisable = Disable;
                        member.UpdatedTime = updateTime;

                        return 0;
                    }

                    LogManager.GetCurrentClassLogger().Error($"Update Member fail id :{Id}");
                    return - 10;
                }
               
                return -11;
            }
        }

        public int SyncRegister(Member member)
        {
            _members.Add(member.Id, member);

            return 0;
        }


        public int SyncUser(string MemberId, string Password, bool EmailConfirmed)
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
    }
}
