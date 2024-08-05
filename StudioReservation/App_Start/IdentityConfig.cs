using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Redis;
using StudioMember.DataModel;
using StudioMember.Service.Contract;
using StudioMember.Service.Proxy;
using StudioReservation.Models;



namespace StudioReservation
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            await configMailTrapasync(message);
            //return Task.FromResult(0);
        }

        private async Task configMailTrapasync(IdentityMessage message)
        {
            var fromAddress = new MailAddress("qylit89@gmail.com", "YL");
            var toAddress = new MailAddress(message.Destination, "ToN");
            string fromPassword = "8fc371316cc6c7";
            string subject = message.Subject;
            string body = message.Body;

            var smtp = new SmtpClient
            {
                Host = "sandbox.smtp.mailtrap.io",
                Port = 2525,
                EnableSsl = true,
                //DeliveryMethod = SmtpDeliveryMethod.Network,
                //UseDefaultCredentials = false,
                Credentials = new NetworkCredential("74f352eff30589", fromPassword)
            };
            using (var msg = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(msg);
                await Task.FromResult(0);
            }
        }
    }


    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        IRedisConnection redis = ServiceConnection.RedisConnection;
        IMemberService memberService = ServiceConnection.MemberService;

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var now = DateTime.Now;
            
            var error = base.CreateAsync(user);

            if (error.Result == IdentityResult.Success)
            {
                var member = new Member
                {
                    Id = user.Id,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    Password = user.PasswordHash,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    CreatedTime = now,
                    Ip = string.Empty,
                    Role = "User",
                };

                //var result = memberService.SyncRegister(
                //    member);

                redis.Publish<Member>("sync-register-member", member);

                //if (result != 0) NLog.LogManager.GetCurrentClassLogger().Error($"Sync Register Member Fail -> {user.Email} -> {result}");
            }

            return error;

        }

        public override Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            var error = base.UpdateAsync(user);

            if(error.Result == IdentityResult.Success)
            {


                var result = memberService.SyncUser(
                    MemberId : user.Id,
                    Password : user.PasswordHash,
                    EmailConfirmed : user.EmailConfirmed);

                if (result != 0) NLog.LogManager.GetCurrentClassLogger().Error($"Sync Update Member Fail -> {user.Email} -> {result}");
            }

            return error;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
