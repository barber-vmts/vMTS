using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using vMTS.Models;

namespace vMTS.Controllers
{
    [RequireHttps]
    public class RegistrationController : Controller
    {
               RegistrationModel c = new RegistrationModel();
               RaceLookup Race = new RaceLookup();
               StateList States = new StateList();
               NameSuffix Suffix = new NameSuffix();
               CustomErrors CE = new CustomErrors();
               CommunicationModel CM = new CommunicationModel();
               AddressValidation AV = new AddressValidation();
        GeneralModel GM = new GeneralModel();

        // GET: Registration
        public ActionResult Index()
        {
            string error ="";
            List <Class_Schedule_view> list = new List<Class_Schedule_view>();
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = from c in db.Class_Schedule_views
                              where c.CLASS_START_DATE >= DateTime.Now// && c.OPEN_SEATS >= 1
                              orderby c.CLASS_START_DATE
                              group c by c.CLASS_TYPE into newGroup
                              select newGroup.ToList();

                    foreach (var e in sql)
                    {
                        foreach (var cg in e)
                            list.Add(new Class_Schedule_view
                            {
                                CLASS_ID = cg.CLASS_ID,
                                CLASS_TYPE = cg.CLASS_TYPE,
                                ClassDay = cg.ClassDay,
                                OPEN_SEATS = cg.OPEN_SEATS,
                                //CLASS_DESCRIPTION = cg.CLASS_DESCRIPTION,
                                CLASS_FEE = cg.CLASS_FEE,
                                CLASS_START_DATE = cg.CLASS_START_DATE,
                                CLASS_START_TIME = cg.CLASS_START_TIME,
                                CLASS_END_TIME = cg.CLASS_END_TIME,
                                C1 = cg.C1,
                                C2 = cg.C2
                                //,COURSE_NUMBER = e.COURSE_NUMBER
                            });
                    }
                }

                //List<InstructorClasses> ic = new List<InstructorClasses>();
                //ic = GM.GetInstructorClass();

                //foreach(var i in ic)-
                //{
                //    string clDay = list.IndexOf(i.ClassDates.ToString());
                //}

            }
            catch (Exception e)
            {
                error = e.Message;
            }

            var classesGrouped = from c in list
                                 group c by c.CLASS_TYPE into ct
                                 select new Group<string, Class_Schedule_view> { Key = ct.Key, Values = ct };

            var tuple = new Tuple<List<Group<string,Class_Schedule_view>>, List<CourseDescriptions>, List<InstructorClasses>>(classesGrouped.ToList(), GM.GetCourseDescriptions(),GM.GetInstructorClass());

            return View(tuple);
        }

        public ActionResult Register(Int32 id)
        {
            c.NameSuffix = Suffix.SuffixSelectList(); 
            c.RaceCodes = Race.RaceSelectList();
            c.States = States.StateSelectList();

           string Class = "";
            string ClassType = "";
           DateTime? Start;
            using (vmts_dataDataContext db = new vmts_dataDataContext())
            {
            var sql = (from cl in db.Class_Schedule_views
                       where cl.CLASS_ID==id
                       select (cl)).FirstOrDefault();
                ClassType = sql.CLASS_TYPE;
                Class = "(" + sql.ClassDay + ")";
                Start = sql.CLASS_START_DATE;
            }


            ViewBag.ClassRegistration = ClassType;
            ViewBag.ClassId = id;
            ViewBag.StartDate = String.Format("{0:d}",Start);
            ViewBag.Title = ClassType + Class;
            return View(c);
        }
        
        public ActionResult RegistrationConfirmation(Int64 id)
        {
            c.Confirmation = c.GetConfirmation(id);
            try { 
            c.RegistrationPayment = c.GetRegistrationPayment(id);

            ViewBag.ClassRegistration = c.GetConfirmation(id).FirstOrDefault().CLASS_DAY;
            ViewBag.ClassId = c.GetConfirmation(id).FirstOrDefault().CLASS_ID;
            ViewBag.Title = "Course Registration " + c.GetConfirmation(id).FirstOrDefault().CLASS_DAY;
            ViewBag.RegNum = id;


            }
            catch
            {
                CE.BadRequest();
            }
            return View(c);
        }

        public ActionResult PrintRegistration(Int64 id)
        {
            c.Confirmation = c.GetConfirmation(id);
            var p = c.GetConfirmation(id).FirstOrDefault().RECEIPT;
            c.RegistrationPayment = c.GetRegistrationPayment(p);

            ViewBag.ClassRegistration = c.GetConfirmation(id).FirstOrDefault().CLASS_DAY;
            ViewBag.ClassId = c.GetConfirmation(id).FirstOrDefault().CLASS_ID;
            ViewBag.Title = "vMTS Registration " + c.GetConfirmation(id).FirstOrDefault().CLASS_DAY;
            return View(c);
        }

        public ActionResult _RegistrationPayment(Int64 id)
        {
            var p = c.GetConfirmation(id).FirstOrDefault().RECEIPT;
            c.RegistrationPayment = c.GetRegistrationPayment(p);
            
            return View(c);
        }

        public string NewClassRegistration(Int32 COURSE_ID, string FirstName, string MiddleName, string LastName, string Suffix, string Address1, string Address2, string City, string inputState, string Zip, string Gender, Int32 inputRace, string Phone, string Email, DateTime DOB, Int32 AGE, string Eval, string MOTOR_YR, string MOTOR_MK, string MOTOR_MD, string DL_NUM, string inputDLState, string PaymentType, string CardName, string CardNum, string CardMonth, string CardYear, string CardCVV, string PromoCode)
        {
            string msg = "";
            using (vmts_dataDataContext db = new vmts_dataDataContext())
            {
                try
                {
                    if(VerifyClassOpenings(COURSE_ID) > 0)
                    {

                    Int64? OrderNum = 0;

                        db.NewRegistration(COURSE_ID, FirstName, MiddleName, LastName, Suffix, Address1, Address2, City, inputState, Zip, Gender, inputRace, Phone, Email, DOB, AGE, Eval, MOTOR_YR, MOTOR_MK, MOTOR_MD, DL_NUM, inputDLState, PaymentType, CardName, CardNum, CardMonth, CardYear, CardCVV, PromoCode, GM.GetPromoCodeValue(PromoCode), ref OrderNum);

                    if (OrderNum.Value > 0)
                    {
                        msg = OrderNum.ToString();

                        CM.SendRegistrationMessage(OrderNum.Value);

                    }
                    }
                    else
                    {
                        msg = "Looks like this class is full, we apologize for the inconvience.  Please select another class date.";
                    }
                }
                catch (Exception e)
                {
                    msg = "Error Loading Registration: Check your entries and try again."; //+e.Message + " " + e.StackTrace;

                    CommunicationModel C = new CommunicationModel();
                    C.SendErrorEmail(e.Message + Environment.NewLine + "Student Name: " + FirstName + " " + LastName, "NewClassRegistration");

                    C.LogErrorToDB("NewClassRegistration", e.Message, e.StackTrace);

                }
            }
            return msg;
        }

        public Int32 VerifyClassOpenings(int COURSE_ID)
        {
            Int32 msg;
            msg = c.VerifyClassOpenings(COURSE_ID);
            return msg;
        }
        public string VerifyPromoCode(string code)
        {
            string msg;

            msg = GM.VerifyPromoCode(code);

            return msg;
        }

        public string ConfirmClassRegistration(Int64 RECEIPT)
        {
            string result = "";

            try
            {
                result = c.RegistrationConfirm(RECEIPT);
            }
            catch
            {
                result = "Fail";
            }

            return result;
        }
        public string AddressValidation(string a, string c, string s, string z)
        {
            XmlDocument r = new XmlDocument();

            // Query the USPS Address API
            r = AV.ValidateAddress(a,c,s, z);

            // Return the XML response
            return r.InnerXml;
        }
    }
}