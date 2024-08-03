﻿
using StudioMember.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StudioMember.Service.Contract
{
    [ServiceContract]
    public interface IMemberService : IDisposable
    {
        [OperationContract]
        MemberViewModel FindByUserName(string Username);

        [OperationContract]
        MemberViewModel Find(string Id);

        [OperationContract]
        MembersViewModel GetAll();

        [OperationContract]
        int Update(string Id,string Email, string PhoneNumber);

        [OperationContract]
        int SyncRegister(Member member);

        [OperationContract]
        int UpdateUser(string MemberId , string Password,bool EmailConfirmed);
  

    }
}
