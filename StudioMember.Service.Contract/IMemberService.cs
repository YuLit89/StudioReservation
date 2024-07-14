
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StudioMember.Service.Contract.Models;
using StudioReservation.Models;
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
        Tuple<int, SignInStatus> Login(string Email , string Password,bool RememberMe);

        [OperationContract]
        Tuple<int, IdentityResult> Register(string Email,string Password,string ConfirmPassword,string ContactNumber,string FullName);

        [OperationContract]
        Tuple<int, IdentityResult> ConfirmEmail(string UserId, string Code);

        [OperationContract]
        int ForgotPassword(string Email);

        [OperationContract]
        Tuple<int,IdentityResult> ResetPassword(string Email,string Password,string ConfirmPassword,string Code);

        //[OperationContract]

    }
}
