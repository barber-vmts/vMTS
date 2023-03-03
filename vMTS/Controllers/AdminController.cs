using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using vMTS.Models;

namespace vMTS.Controllers
{
    [SessionExpire]
    //[RequireHttps]
    [RoleName]
    public class AdminController : Controller
    {
        LoginPartialViewModel lp = new LoginPartialViewModel();
        AdministrationModel am = new AdministrationModel();
        ClassAdministration ca = new ClassAdministration();
        RegistrationAdministration ra = new RegistrationAdministration();
        AccountController i = new AccountController();
        Sponsors sp = new Sponsors();
        Exports export = new Exports();
        PromoCodes pc = new PromoCodes();

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Title = "vMTS Administration";
            ViewBag.UserName = "Signed In As: " + SignInManager.CurrentUser;
            ViewBag.Role = "User Role: " + SignInManager.CurrentRoleName + " Validated: " + SignInManager.CurrentValid;

            UsersModel u = new UsersModel();
            ViewData["RoleList"] = u.LoadRoles();

            return View(am.GetAllUsers());
        }

        #region Users
        public ActionResult _users()
        {

            return View(am.GetAllUsers());
        }

        //[Role]
        public ActionResult _edituser(Int32 id)
        {

            return View();
        }

        //[Role]
        public ActionResult _adduser()
        {
            //u.RoleName = u.LoadRoles();
            return View();
        }
        [ValidateAntiForgeryToken]
        public JsonResult NewUser(string Email, string RoleName)
        {
            string r = "Fail";
            string em = @"^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$";

            Regex reg = new Regex(em, RegexOptions.IgnoreCase);
            Match mat = reg.Match(Email);

            if (mat.Success)
            {
                if (RoleName.Length > 0)
                {
                    r = am.AddUser(Email, RoleName);
                }
                else
                {
                    r = "Select a Role";
                }
            }
            else
            {
                r = "Email is invalid";
            }

            return Json(r);
        }
        #endregion

        #region Registrations
        public ActionResult registrations()
        {
            ra.CurrentRegCounts = ra.Get_CurrentRegCounts();

            return View(ra);
        }

        //[Role]
        public ActionResult students(Int32 id)
        {
            ra.StudentsByClass = ra.Get_StudentsByClass(id);

            return View(ra);
        }

        //[Role]
        public ActionResult studentreg(Int64 id)
        {
            ra.StudentRegistration = ra.Get_StudentRegistration(id);

            ra.RegistrationPayment = ra.Get_RegistrationPayment(id);

            return View(ra);
        }
        
        public ActionResult studentedit(Int64 id)
        {
            RaceLookup Race = new RaceLookup();
            StateList States = new StateList();
            NameSuffix Suffix = new NameSuffix();
            List<SelectListItem> Gender = new List<SelectListItem>();
            Gender.Add(new SelectListItem
            {
                Value = "Male",
                Text = "M"

            });
            Gender.Add( new SelectListItem
            {
                Value = "Female",
                Text = "F"

            });
            Gender.Add(new SelectListItem
            {
                Value = "",
                Text = ""

            });
            List<SelectListItem> YesNo = new List<SelectListItem>();
            Gender.Add(new SelectListItem
            {
                Value = "Y",
                Text = "Yes"

            });
            Gender.Add(new SelectListItem
            {
                Value = "N",
                Text = "No"

            });

            ViewBag.Suffix = Suffix.SuffixSelectList();
            ViewBag.RaceCodes = Race.RaceSelectList();
            ViewBag.States = States.StateSelectList();
            ViewBag.Gender = new SelectList(Gender, "Value", "Text");
            ViewBag.YesNo = new SelectList(YesNo, "Value", "Text");

            ra.StudentRegistration = ra.Get_StudentRegistration(id);

            ra.RegistrationPayment = ra.Get_RegistrationPayment(id);
            

            return View(ra);
        }

        public PartialViewResult _studentclass()
        {

            return PartialView();
        }

        //[Role]
        public PartialViewResult _print_studentreg(Int64 id)
        {
            ra.StudentRegistration = ra.Get_StudentRegistration(id);

            ra.RegistrationPayment = ra.Get_RegistrationPayment(id);

            return PartialView(ra);
        }

        public JsonResult RegConfirmation(Int64 r)
        {
            RegistrationModel c = new RegistrationModel();
            string result;

            result = c.RegistrationConfirm(r);

            return Json(result);
        }

        public JsonResult ProcessPayment(Int64 r)
        {
            string result;

            result = ra.ProcessPayment(r);

            return Json(result);
        }


        //[Role]
        public ActionResult regpayment()
        {

            //ra.RegistrationPayment = ra.Get_RegistrationPayment(id);

            return View();
        }
        public JsonResult ExportToCSV(Int64 r)
        {
            string msg;
            try
            {
                export.CreateCSV(r);

                msg = "Success";
            }
            catch (Exception e)
            {
                msg = "Error: " + e.Message;
            }

            return Json(msg);
        }

        public FileResult ExportClassToCSV(int COURSE_ID)
        {
            var exportCe = export.CreateClassCSV(COURSE_ID);
            string fileName = "class.xlsx";
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[19] {
                                            new DataColumn("First Name"),
                                            new DataColumn("Middle Name"),
                                            new DataColumn("Last Name"),
                                            new DataColumn("Suffix"),
                                            new DataColumn("Nick Name"),
                                            new DataColumn("Contact Type"),
                                            new DataColumn("Phone"),
                                            new DataColumn("Gender"),
                                            new DataColumn("Date Of Birth"),
                                            new DataColumn("Race"),
                                            new DataColumn("Email"),
                                            new DataColumn("T-Shirt Size"),
                                            new DataColumn("Address Line 1"),
                                            new DataColumn("Address Line 2"),
                                            new DataColumn("City"),
                                            new DataColumn("State"),
                                            new DataColumn("Zip"),
                                            new DataColumn("Driver License State of Issue"),
                                            new DataColumn("Driver License Number")
            });

            if (exportCe.Any())
            {
                fileName = exportCe.FirstOrDefault().ClassDay + ".xlsx";
                foreach (var customer in exportCe)
                {
                    var dateOfBirth = "";
                    if (!string.IsNullOrEmpty(customer.DateofBirth))
                    {
                        DateTime date = DateTime.Parse(customer.DateofBirth);
                        dateOfBirth = date.ToString("MM/dd/yyyy");
                    }

                    dt.Rows.Add(customer.FirstName, customer.MiddleName, customer.LastName, customer.Suffix, customer.Nickname, customer.ContactType, customer.Phone, customer.Gender, dateOfBirth, customer.Race, customer.Email, customer.TShirtSize,
                        customer.Street1, customer.Street2, customer.City, customer.State, customer.ZIP, customer.DriverLicenseState, customer.DriverLicenseNumber);
                }
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public JsonResult CancelRegistration(Int64 r)
        {
            string result;

            result = ra.CancelReg(r);

            return Json(result);
        }

        public PartialViewResult _classchange(Int64 id)
        {
            ra.StudentRegistration = ra.Get_StudentRegistration(id);

            ra.ScheduleDates = ra.Get_ScheduleDates("Basic Rider Course I");

            return PartialView(ra);
        }

        public JsonResult SwitchClasses(Int64 r, Int32 o, Int32 n)
        {
            string result;

            result = ra.ChangeClass(r, o, n);

            return Json(result);
        }

        #endregion

        #region Carousel
        public ActionResult carousel()
        {
            //c.CurrentImages = c.GetCurrentImages();
            //c.RemoveImage = c.GetAnImage("");

            return View();
        }

        #endregion

        #region Sponsors
        public ActionResult sponsors()
        {
            sp.CurrentSponsors = sp.GetCurrentSponsors();

            return View(sp);
        }
        #endregion

        #region RiderCoaches
        public ActionResult instructors()
        {
            //rc.CurrentInstructors = rc.GetCurrentInstructors();

            return View();
        }

        #endregion

        #region PromoCodes
        public ActionResult promocodes()
        {
            pc.AllCodes = pc.GetAllPromoCodes();

            return View(pc);
        }
        #endregion

        public ActionResult errorlog()
        {

            ViewBag.Title = "vMTS Error Log";

            var log = new List<errorLogging>();
            using (vmts_dataDataContext db = new vmts_dataDataContext())
            {
                var sql = (from c in db.errorLoggings
                           select (c)).ToList();
                foreach (var r in sql)
                {
                    log.Add(new errorLogging
                    {
                        logID = r.logID,
                        logDate = r.logDate,
                        logMethod = r.logMethod,
                        logMessage = r.logMessage,
                        logStackTrace = r.logStackTrace
                    });
                }
            }

            return View(log);
        }
    }
}