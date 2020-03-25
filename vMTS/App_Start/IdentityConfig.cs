using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using vMTS.Models;
using vMTS.App_Start;

namespace vMTS
{

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : ApplicationUser
    {
        //public ApplicationUserManager(IUserStore<ApplicationUser> store)
        //    : base(store)
        //{
        //}

    }

    // Configure the application sign-in manager which is used in this application.
    //public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    //{
        //public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        //    : base(userManager, authenticationManager)
        //{
        //}

        //public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        //{
        //    return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        //}

        //public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        //{
        //    return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        //}
    //}
}
