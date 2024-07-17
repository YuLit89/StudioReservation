

using StudioMember.Service.Contract.Models;
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
        Member FindByUserName(string Username);

        [OperationContract]
        Member Find(string Id);

        [OperationContract]
        List<Member> GetAll();

        [OperationContract]
        int Update(string Id,string Email, string PhoneNumber);

        [OperationContract]
        int Register(string MemberId,string Email,bool EmailConfirmed,string Password,string PhoneNumber,string UserName);

        [OperationContract]
        int UpdateUser(string MemberId , string Password,bool EmailConfirmed);
  

    }
}
