using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vMTS.Models;

namespace vMTS.Controllers
{
    //[RequireHttps]
    //[SessionExpire]
    //[RoleName]
    public class AdminScheduleController : Controller
    {
        AdministrationModel am = new AdministrationModel();
        ClassAdministration ca = new ClassAdministration();
        CommunicationModel com = new CommunicationModel();
        
        // GET: AdminSchedule
        public ActionResult Index()
        {
            ca.ClassType = ca.Get_ClassType("Basic Rider Course I");
            ca.CourseLocation = ca.Get_CourseLocation("Aladdin Temp-Rite");
            ca.CourseNumber = ca.Get_CourseNumber(1);
            ca.StartClassTimes = ca.Get_ClassTimes("07:00:00");
            ca.EndClassTimes = ca.Get_ClassTimes("19:00:00");
            
            ca.CurrentSchedule = am.GetSchedule();

            return View(ca);
        }
        public ActionResult _Schedule()
        {

            return View(am.GetSchedule());
        }

       public ActionResult _AddSchedule()
        {

            return View(ca);
        }

        public ActionResult _EditSchedule(Int32 id)
        {

            var list = new List<Class_Schedule_view>();
            list = ca.GetSchedule_edit(id);

            ca.EditSchedule = list;
            ca.ClassType = ca.Get_ClassType(list[0].CLASS_TYPE);
            ca.CourseLocation = ca.Get_CourseLocation(list[0].CLASS_LOCATION);
            ca.CourseNumber = ca.Get_CourseNumber(Convert.ToInt32(list[0].COURSE_NUMBER));
            ca.StartClassTimes = ca.Get_ClassTimes(list[0].ST_SELECTED.ToString());
            ca.EndClassTimes = ca.Get_ClassTimes(list[0].ET_SELECTED.ToString());

            return PartialView(ca);
        }
        public ActionResult _DeleteClass(Int32 id)
        {

            var list = new List<Class_Schedule_view>();
            list = ca.GetSchedule_edit(id);

            ca.EditSchedule = list;
            ca.ClassType = ca.Get_ClassType(list[0].CLASS_TYPE);
            ca.CourseLocation = ca.Get_CourseLocation(list[0].CLASS_LOCATION);
            ca.CourseNumber = ca.Get_CourseNumber(Convert.ToInt32(list[0].COURSE_NUMBER));
            //ca.StartClassTimes = ca.Get_ClassTimes(list[0].CLASS_START_TIME);
            //ca.EndClassTimes = ca.Get_ClassTimes(list[0].CLASS_END_TIME);

            return PartialView(ca);
        }
        public ActionResult _ClearSchedule()
        {
            try {
                string e = SignInManager.CurrentUser;
                string c = ca.GenerateCode();
                ViewBag.User = e;

                SendConfirmationCode(e, c);
            }
            catch
            {
                RedirectToAction("Login");
            }

            return PartialView();
        }
       public JsonResult ClassScheduleChanges(Int32 CLASS_ID, string CLASS_TYPE, string CLASS_START_DATE, string CLASS_START_TIME, string CLASS_END_DATE, string CLASS_END_TIME, string LOCATION, Int32 COURSE_NUMBER, Int32 OPEN_SEATS, Decimal CLASS_FEE)
        {
            var msg = "";

            msg = ca.ScheduleUpdates(CLASS_ID, CLASS_TYPE, CLASS_START_DATE, CLASS_START_TIME, CLASS_END_DATE, CLASS_END_TIME, LOCATION, COURSE_NUMBER, OPEN_SEATS, CLASS_FEE);

            return Json(msg);
        }

        public JsonResult CancelClass(Int32 CLASS_ID)
        {
            var msg = "";

            msg = ca.DeleteClass(CLASS_ID);

            return Json(msg);
        }

        public JsonResult SendConfirmationCode(string email, string code)
        {
            var msg = "";

            msg = com.SendConfirmationCode(email, code);

                Session["SchedCode"] = code;

            return Json(msg);
        }

        public JsonResult CheckConfirmationCode(string code)
        {
            var msg = "";
            bool match = ca.ReadTempCode(code);
            if (match) { msg = "Code Matched"; } else { msg = "No Match"; }

            return Json(msg);
        }

        public JsonResult ClearSchedule()
        {
            var msg = "";
                 ApplicationUser uc = new ApplicationUser();
                bool match = uc.ReadAdminTempContract(Session["SchedCode"].ToString());

                if (match)
                {
                    msg = ca.ClearSchedule();

                    Session.Remove("SchedCode");

                    //uc.RemoveAdminTempContract();
                }
                else
                {
                    msg = "Code did not match";
                }
            return Json(msg);
        }

    }
}