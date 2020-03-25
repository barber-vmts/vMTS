using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vMTS.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            ViewBag.Title = "We apologize for the inconvenience.";
            ViewBag.Message = "If you feel this is not correct, contact the site administrator.";

            return View("Error");
        }

        public ActionResult NotAuthorized()
        {
            ViewBag.Title = "You are not authorized to view this content.";
            ViewBag.Message = "If you feel this is not correct, contact the site administrator.";

            Response.StatusCode = 401;

            return View("Error");
        }
    }

}