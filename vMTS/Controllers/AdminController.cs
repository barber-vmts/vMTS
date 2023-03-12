using ClosedXML.Excel;
using Microsoft.Ajax.Utilities;
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
            Race.RaceSelectList();
            StateList States = new StateList();
            States.StateSelectList();
            StateList DLStates = new StateList();
            DLStates.StateSelectList();
            NameSuffix Suffix = new NameSuffix();
            Suffix.SuffixSelectList();
            List<SelectListItem> Gender = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "Male",
                    Text = "M"

                },
                new SelectListItem
                {
                    Value = "Female",
                    Text = "F"

                },
                new SelectListItem
                {
                    Value = "",
                    Text = ""

                }
            };
            List<SelectListItem> Eval = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "Y",
                    Text = "Yes"

                },
                new SelectListItem
                {
                    Value = "N",
                    Text = "No"

                }
            };
            List<SelectListItem> CardType = new List<SelectListItem> {
                new SelectListItem
                {
                    Value = "Visa",
                    Text = "Visa"

                },
                new SelectListItem
                {
                    Value = "MasterCard",
                    Text = "Master Card"

                },
                new SelectListItem
                {
                    Value = "Discover",
                    Text = "Discover"

                },
                new SelectListItem
                {
                    Value = "Gift",
                    Text = "Gift Card/Certificate"
                }
            };

            ra.StudentRegistration = ra.Get_StudentRegistration(id);
            ra.RegistrationPayment = ra.Get_RegistrationPayment(id);

            var dob = Convert.ToDateTime(ra.StudentRegistration.First().DOB);
            ra.StudentRegistration.First().DOB = dob.ToString("MM-dd-yyyy");

            foreach (var project in States.list.Where(p => p.Value == ra.StudentRegistration.First().STATE))
            {
                project.Selected = true;
            }
            foreach (var project in DLStates.list.Where(p => p.Value == ra.StudentRegistration.First().DL_ST))
            {
                project.Selected = true;
            }

            foreach (var project in Suffix.list.Where(p => p.Value == ra.StudentRegistration.First().SUFFIX))
            {
                project.Selected = true;
            }

            foreach (var project in Race.list.Where(p => p.Text == ra.StudentRegistration.First().RACE))
            {
                project.Selected = true;
            }

            foreach (var project in Gender.Where(p => p.Value == ra.StudentRegistration.First().GENDER))
            {
                project.Selected = true;
            }

            foreach (var project in Eval.Where(p => p.Value == ra.StudentRegistration.First().EVAL))
            {
                project.Selected = true;
            }
            foreach (var project in CardType.Where(p => p.Value == ra.RegistrationPayment.First().PMT_TYPE))
            {
                project.Selected = true;
            }

            DateTime? Start;
            using (vmts_dataDataContext db = new vmts_dataDataContext())
            {
                var sql = (from cl in db.Class_Schedule_views
                           where cl.CLASS_ID == ra.StudentRegistration.First().CLASS_ID
                           select (cl)).FirstOrDefault();
                Start = sql.CLASS_START_DATE;
            }

            ViewBag.Suffix = Suffix.list;
            ViewBag.RaceCodes = Race.list;
            ViewBag.States = States.list;
            ViewBag.Gender = Gender;
            ViewBag.YesNo = Eval;
            ViewBag.CardType = CardType;
            ViewBag.DLStates = DLStates.list;
            ViewBag.StartDate = String.Format("{0:d}", Start);

            return View(ra);
        }

        public ActionResult UpdateRegistration(Int32? ADDRESS_ID, Int32? STUDENT_ID, Int32? PMT_ID, string FirstName, string MiddleName, string LastName, string Suffix, string Address1, string Address2, string City, string State, string Zip, string Gender, Int32? Race, string Phone, string Email, DateTime? DOB, Int32? AGE, string Eval, string MOTOR_YR, string MOTOR_MK, string MOTOR_MD, string DL_NUM, string DL_ST, string PMT_TYPE, string CardName, string CardNum, string CardMonth, string CardYear, string CardCVV)
        {


            using (vmts_dataDataContext db = new vmts_dataDataContext())
            {
                try
                {
                    if (Race == 0)
                        Race = 8;
                    db.UpdateRegistration(STUDENT_ID, ADDRESS_ID, PMT_ID, FirstName, MiddleName, LastName, Suffix, Address1, Address2, City, State, Zip, Gender, Race, Phone, Email, DOB, AGE, Eval, MOTOR_YR, MOTOR_MK, MOTOR_MD, DL_NUM, DL_ST, PMT_TYPE, CardName, CardNum, CardMonth, CardYear, CardCVV);
                    return RedirectToAction("registrations");
                }
                catch (Exception e)
                {
                    string msg = "";
                    msg = "Error Updating Registration: Check your entries and try again."; //+e.Message + " " + e.StackTrace;

                    CommunicationModel C = new CommunicationModel();
                    C.SendErrorEmail(e.Message + Environment.NewLine + "Student Name: " + FirstName + " " + LastName, "EditClassRegistration");

                    C.LogErrorToDB("EditClassRegistration", e.Message, e.StackTrace);
                }
            }
            return RedirectToAction("Error","Home");
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