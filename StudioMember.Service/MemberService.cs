﻿
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
        Func<string, string, string,bool,DateTime, int> _update;
        Func<string, bool, int> _updateDisable;
        Func<string, DateTime, string, int> _updateSubInfo;

        Dictionary<string, Member> _members = new Dictionary<string, Member>();

        ConcurrentDictionary<string, object> _sync = new ConcurrentDictionary<string, object>();

        public MemberService(
            Func<IEnumerable<Member>> getAll,
            Func<IEnumerable<MemberRole>> getAllRole,
            Func<string, string, string, bool,DateTime, int> update,
            Func<string,bool,int> updateDisable,
            Func<string,DateTime,string,int> updateSubInfo)
        {
            _getAll = getAll;
            _update = update;
            _updateDisable = updateDisable;
            _updateSubInfo = updateSubInfo;
            
            
            foreach (var m in _getAll())
            {
                _members[m.Id] = m;
            }

            foreach(var r in getAllRole())
            {
                Member m;
                if(_members.TryGetValue(r.UserId,out m))
                {
                    m.Role = r.Role;
                }
            }

            Console.WriteLine($"{DateTime.Now} || init DATA Total {_members.Count()}");
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

        public int UpdateMemberStatus(string MemberId, bool isDisable)
        {
            Member member;
             if(_members.TryGetValue(MemberId,out member))
            {
                //if (member.Role != "Admin") return -11;

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

        public int UpdateRegisterSubInfo(string Id, DateTime CreateTime,string Ip)
        {
            Member m;
            if(_members.TryGetValue(Id,out m))
            {
                var result = _updateSubInfo(Id, CreateTime, Ip);
                if(result == 0)
                {
                    m.Ip = Ip;
                    m.CreatedTime = CreateTime;

                    return 0;
                }
            }

            return -10;
        }
    }
}
