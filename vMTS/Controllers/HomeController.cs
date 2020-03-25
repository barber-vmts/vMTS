using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vMTS.Models;

namespace vMTS.Controllers
{
    
    public class HomeController : Controller
    {
        GeneralModel g = new GeneralModel();

        public ActionResult Index()
        {
            ViewBag.title = "volunteer motorcycle training services - vMTS";

            ViewBag.description = "Volunteer Motorcycle Training Services is a Hendersonville, TN based organization that offers courses in motorcycle riding for people in Middle Tennessee.";

            g.GetCarouselImages = g.GetAllImages();

            return View(g);
        }

        public ActionResult courses()
        {
            ViewBag.title = "Rider Courses";
            ViewBag.description = "Rider Courses for Volunteer Motorcycle Training Services in Hendersonville, TN offer the best training in motorcycle riding for people in Middle Tennessee.";

            return View(g.GetCourseDescriptions());
        }

        public ActionResult about()
        {
            ViewBag.title = "About vMTS";
            ViewBag.description = "About Volunteer Motorcycle Training Services in Nashville/Hendersonville, TN";

            return View();
        }


        public ActionResult fees()
        {
            ViewBag.title = "Course Fees";

            ViewBag.description = "Volunteer Motorcycle Training Services in Hendersonville, TN has the cheapest fees in middle tennessee";

            return View(g.GetCourseDescriptions());
        }

        public ActionResult contact()
        {
            ViewBag.title = "Contact Us for More Information";

            ViewBag.description = "Contact Volunteer Motorcycle Training Services in Hendersonville, TN for More Information about motorcycle training courses";

            return View();
        }

        public ActionResult instructors()
        {
            ViewBag.title = "Experienced Rider Coaches";
            ViewBag.description = "vMTS offers the best motorcycle training in Hendersonville and middle TN";

            return View(g.GetAllInstructors());
        }

        public ActionResult sponsors()
        {
            ViewBag.title = "Sponsors";
            ViewBag.description = "Volunteer Motorcycle Training Services in Hendersonville, TN would like to thank our motocycle sponsors";

            return View(g.GetAllSponsors());
        }
        public ActionResult _location()
        {
            ViewBag.title = "Location";
            ViewBag.description = "Course Location Hendersonville, TN 37075";

            return View();
        }

        public ActionResult _media()
        {

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}