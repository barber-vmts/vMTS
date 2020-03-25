using System.Web;
using System.Web.Mvc;
using vMTS.Controllers;
using vMTS.Models;

namespace vMTS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
    public class SessionExpireAttribute : ActionFilterAttribute
    {
                 AccountController a = new AccountController();
       public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //a.LogActivityDate(HttpContext.Current.Session["User"].ToString());

            HttpContext ctx = HttpContext.Current;

           // check  sessions here
            if (HttpContext.Current.Session["User"] == null)
            {
                filterContext.Result = new RedirectResult("~/account/login");


                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class RoleNameAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //AccountController a = new AccountController();
            //a.LogActivityDate();

            HttpContext ctx = HttpContext.Current;

            // check  sessions here
            if (SignInManager.CurrentRoleName != "admin")
            {
                filterContext.Result = new RedirectResult("~/Error/NotAuthorized");
                //filterContext.Result = new RedirectResult("~/account/login");

                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
