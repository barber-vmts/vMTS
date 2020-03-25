using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using vMTS.Models;

namespace vMTS.Controllers
{
    [RequireHttps]
    [Authorize]
    public class AccountController : Controller
    {
        //
            LoginPartialViewModel lp = new LoginPartialViewModel();
            LoginViewModel model = new LoginViewModel();
            //string site = "/qa";

            // GET: /Account/Login
            [AllowAnonymous]
            public ActionResult _LoginPartial()
            {
                ViewBag.User = Session["User"];
               return View();
            }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {

            ViewBag.ReturnUrl = "/admin/index";




            return View();
        }

        
         //POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string result = await SignInManager.PasswordSignInAsync(model.Email);
            switch (result)
            {
                case "Pass":
                    Session["User"] = SignInManager.CurrentUser;
                    LogActivityDate(Session["User"].ToString());
                    return RedirectToLocal(returnUrl);
                case "Code":
                    string codeSent = SendCode(model.Email);
                    await Task.Delay(1000);

                    return RedirectToAction("VerifyCode", new { em = model.Email, ReturnUrl = returnUrl, CodeSent = codeSent });
                case "Not Registered":
                    return RedirectToAction("NotRegistered", new { em = model.Email });
                case "Fail":
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        
         // SEND A VERIFICATION CODE TO THE USER
        public string SendCode(string em)
        {
            string r;
            try {
                // GENERATE A NEW CODE TO VERIFY
                string newCode = Guid.NewGuid().ToString();
                newCode = newCode.Substring(0, 5).ToUpper();

                // SEND THE CODE TO THE EMAIL USED TO LOGIN
                CommunicationModel cm = new CommunicationModel();
                r = cm.SendConfirmationCode(em, newCode);

                if (r == "Code Sent") { 
                    // SET THE CODE IN THE TEMPORARY COOKIE
                    ApplicationUser au = new ApplicationUser();
                    au.SetTempContract(newCode,em);
                }
                else
                {
                    r = "Code Could Not Be Sent, Check with the Site Administrator.";
                }
                
                //r = "Code Sent";
            }
            catch
            {
                r = "Code Could Not Be Sent, Check with the Site Administrator.";
            }

            return r;
        }

         //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string em, string returnUrl, string codeSent)
        {
            ViewBag.Title = "Verify Code";
            await Task.Delay(10);

            if (codeSent != "Code Sent")
            {
                ViewBag.Title = codeSent;
            }

            return View(new VerifyCodeViewModel { ReturnUrl = returnUrl });

        }


        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            ViewBag.Title = "Verify Code";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Code);
            switch (result)
            {
                case "Success":
                    return RedirectToLocal("/admin/index");
                //case SignInStatus.LockedOut:
                //    return View("Lockout");
                case "Fail":
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult NotRegistered(string em)
        {
            string returnUrl = "Login";

            return View(new NotRegisteredViewModel { Email = em, ReturnUrl = returnUrl });
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            LogActivityDate(Session["User"].ToString());

            Session.RemoveAll();

            Session.Abandon();

            return RedirectToAction("Login", "Account");
        }

        public void SetSession()
        {
            Session["User"] = lp.User;
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        public void LogActivityDate(string user)
        {
            using (ID_dataDataContext db = new ID_dataDataContext())
            {
                try
                {
                    db.UpdateLogin_Date(user);
                    
                }
                catch
                {

                }
            }
        }
        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        public ActionResult RedirectToLocal(string returnUrl)
        {
            try { 
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }
            catch
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            //public override void ExecuteResult(ControllerContext context)
            //{
            //    var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            //    if (UserId != null)
            //    {
            //        properties.Dictionary[XsrfKey] = UserId;
            //    }
            //    context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            //}
        }
        #endregion
    }
}