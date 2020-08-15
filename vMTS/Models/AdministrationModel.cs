using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vMTS.Models
{
        public class UsersModel
        {
            [Required]
            [Display(Name = "Email")]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Role")]
            public SelectList RoleName { get; set; }

            public SelectList LoadRoles()
            {
                string value = "";

                List<SelectListItem> list = new List<SelectListItem>();

                list.Add(new SelectListItem { Value = "admin", Text = "Administrator" });
                list.Add(new SelectListItem { Value = "user", Text = "User" });

                return new SelectList(list, "Value", "Text", value);
            }
        }
    public class AdministrationModel
    {
        #region Users
        public List<UserList_vw> GetAllUsers()
        {
            var l = new List<UserList_vw>();
            try
            {
                using (ID_dataDataContext db = new ID_dataDataContext())
                {
                    //l = db.UserList_vws.Where(y => y.RoleId.Select(x => x.RoleId).Contains("1")).ToList();

                    var sql = (from c in db.UserList_vws
                               select (c)).ToList();
                    foreach (var r in sql)
                    {
                        l.Add(new UserList_vw
                        {
                            RoleName = r.RoleName,
                            UserName = r.UserName,
                            LastActivityDate = r.LastActivityDate
                        });
                    }
                }
            }
            catch (Exception e)
            {
                l.Add(new UserList_vw { RoleName = e.Message });
            }

            return l;
        }

        public string AddUser(string UserName, string RoleName)
        {
            string r = "Fail";
            try
            {
                using (ID_dataDataContext db = new ID_dataDataContext())
                {
                    db.NewUser(UserName, RoleName);
                }

                r = "Pass";
            }
            catch (Exception e)
            {
                r = "That's an error =>: " + e.Message;
            }

            return r;

        }
        #endregion
        public List<Class_Schedule_view> GetSchedule()
        {
            var l = new List<Class_Schedule_view>();
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = (from c in db.Class_Schedule_views
                               orderby c.CLASS_START_DATE descending
                               select (c)).ToList();
                    foreach (var r in sql)
                    {
                        l.Add(new Class_Schedule_view
                        {
                            CLASS_ID = r.CLASS_ID,
                            CLASS_TYPE = r.CLASS_TYPE,
                            ClassDay = r.ClassDay,
                            CLASS_START_DATE = r.CLASS_START_DATE,
                            CLASS_START_TIME = r.CLASS_START_TIME,
                            CLASS_END_DATE = r.CLASS_END_DATE,
                            CLASS_END_TIME = r.CLASS_END_TIME,
                            OPEN_SEATS = r.OPEN_SEATS,
                            CLASS_FEE = r.CLASS_FEE,
                            CLASS_LOCATION = r.CLASS_LOCATION,
                            COURSE_NUMBER = r.COURSE_NUMBER,
                            //CLASS_DESCRIPTION = r.CLASS_DESCRIPTION,
                            ST_SELECTED = r.ST_SELECTED,
                            ET_SELECTED = r.ET_SELECTED
                        });
                    }
                }
            }
            catch (Exception e)
            {
                l.Add(new Class_Schedule_view { CLASS_ID = 0, CLASS_TYPE = e.Message });
            }

            return l;
        }

    }

    #region Schedule
    public class ClassAdministration : Class_Schedule_view
    {
        GeneralModel GM = new GeneralModel();
        public List<Class_Schedule_view> CurrentSchedule { get; set; }
        public List<Class_Schedule_view> EditSchedule { get; set; }
       public SelectList ClassType { get; set; }
        public SelectList CourseLocation { get; set; }
        public SelectList CourseNumber { get; set; }
        public SelectList StartClassTimes { get; set; }
        public SelectList EndClassTimes { get; set; }

        public SelectList Get_ClassType(string value)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach(var c in GM.GetCourseDescriptions())
            {
                list.Add(new SelectListItem { Value = c.CourseName, Text = c.CourseName });
            }

            //list.Add(new SelectListItem { Value = "Basic RiderCourse I", Text = "Basic RiderCourse I" });
            //list.Add(new SelectListItem { Value = "Basic RiderCourse II", Text = "Basic RiderCourse II" });
            //list.Add(new SelectListItem { Value = "Advanced RiderCourse", Text = "Advanced RiderCourse" });
            //list.Add(new SelectListItem { Value = "3 Wheel (Trike) Course", Text = "3 Wheel (Trike) Course" });
            //list.Add(new SelectListItem { Value = "Skills Practice and Improvement", Text = "Skills Practice and Improvement" });

            return new SelectList(list, "Value", "Text",value);
        }

        public SelectList Get_CourseLocation(string value)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem { Value = "Aladdin Temp-Rite", Text = "Aladdin Temp-Rite" });
            //list.Add(new SelectListItem { Value = "1", Text = "Basic Rider Course II" });

            return new SelectList(list, "Value", "Text",value);
        }

        public SelectList Get_CourseNumber(Int32 value)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem { Value = "1", Text = "Course Number I" });
            list.Add(new SelectListItem { Value = "2", Text = "Course Number II" });

            return new SelectList(list, "Value", "Text",value);
        }
        public SelectList Get_ClassTimes(string value)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem { Value = "07:00:00", Text = "7:00AM" });
            list.Add(new SelectListItem { Value = "07:30:00", Text = "7:30AM" });
            list.Add(new SelectListItem { Value = "08:00:00", Text = "8:00AM" });
            list.Add(new SelectListItem { Value = "08:30:00", Text = "8:30AM" });
            list.Add(new SelectListItem { Value = "09:00:00", Text = "9:00AM" });
            list.Add(new SelectListItem { Value = "09:30:00", Text = "9:30AM" });
            list.Add(new SelectListItem { Value = "10:00:00", Text = "10:00AM" });
            list.Add(new SelectListItem { Value = "10:30:00", Text = "10:30AM" });
            list.Add(new SelectListItem { Value = "11:00:00", Text = "11:00AM" });
            list.Add(new SelectListItem { Value = "11:30:00", Text = "11:30AM" });
            list.Add(new SelectListItem { Value = "12:00:00", Text = "12:00PM" });
            list.Add(new SelectListItem { Value = "12:30:00", Text = "12:30PM" });
            list.Add(new SelectListItem { Value = "13:00:00", Text = "1:00PM" });
            list.Add(new SelectListItem { Value = "13:30:00", Text = "1:30PM" });
            list.Add(new SelectListItem { Value = "14:00:00", Text = "2:00PM" });
            list.Add(new SelectListItem { Value = "14:30:00", Text = "2:30PM" });
            list.Add(new SelectListItem { Value = "15:00:00", Text = "3:00PM" });
            list.Add(new SelectListItem { Value = "15:30:00", Text = "3:30PM" });
            list.Add(new SelectListItem { Value = "16:00:00", Text = "4:00PM" });
            list.Add(new SelectListItem { Value = "16:30:00", Text = "4:30PM" });
            list.Add(new SelectListItem { Value = "17:00:00", Text = "5:00PM" });
            list.Add(new SelectListItem { Value = "17:30:00", Text = "5:30PM" });
            list.Add(new SelectListItem { Value = "18:00:00", Text = "6:00PM" });
            list.Add(new SelectListItem { Value = "18:30:00", Text = "6:30PM" });
            list.Add(new SelectListItem { Value = "19:00:00", Text = "7:00PM" });
            list.Add(new SelectListItem { Value = "19:30:00", Text = "7:30PM" });
            list.Add(new SelectListItem { Value = "20:00:00", Text = "8:00PM" });

            return new SelectList(list, "Value", "Text", value);
        }

        public List<Class_Schedule_view> GetSchedule_edit(Int32 id)
        {
            var l = new List<Class_Schedule_view>();
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = (from c in db.Class_Schedule_views
                               where c.CLASS_ID == id
                               select (c)).ToList();
                    foreach (var r in sql)
                    {
                        l.Add(new Class_Schedule_view
                        {
                            CLASS_ID = r.CLASS_ID,
                            CLASS_TYPE = r.CLASS_TYPE,
                            ClassDay = r.ClassDay,
                            CLASS_START_DATE = r.CLASS_START_DATE,
                            CLASS_START_TIME = r.CLASS_START_TIME,
                            CLASS_END_DATE = r.CLASS_END_DATE,
                            CLASS_END_TIME = r.CLASS_END_TIME,
                            OPEN_SEATS = r.OPEN_SEATS,
                            CLASS_FEE = r.CLASS_FEE,
                            CLASS_LOCATION = r.CLASS_LOCATION,
                            COURSE_NUMBER = r.COURSE_NUMBER,
                            //CLASS_DESCRIPTION = r.CLASS_DESCRIPTION,
                            ST_SELECTED = r.ST_SELECTED,
                            ET_SELECTED = r.ET_SELECTED
                       });
                    }
                }
            }
            catch (Exception e)
            {
                l.Add(new Class_Schedule_view { CLASS_ID = 0, CLASS_TYPE = e.Message });
            }

            return l;
        }

        public string ScheduleUpdates(Int32 CLASS_ID,string CLASS_TYPE,string CLASS_START_DATE, string CLASS_START_TIME,string CLASS_END_DATE,string CLASS_END_TIME,string LOCATION,Int32 COURSE_NUMBER,Int32 OPEN_SEATS,Decimal CLASS_FEE)
        {
            string result = "";
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    db.Class_Schedule_Changes(CLASS_ID, CLASS_TYPE, Convert.ToDateTime(CLASS_START_DATE),CLASS_START_TIME, Convert.ToDateTime(CLASS_END_DATE),CLASS_END_TIME, LOCATION, COURSE_NUMBER, OPEN_SEATS, CLASS_FEE);
                }

                result = "Schedule Changed Successfully";
            }
            catch (Exception e)
            {
                result = "That's an error: " + e.HelpLink;
            }
            return result;
        }

        public string DeleteClass(Int32 CLASS_ID)
        {
            string result = "";
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    db.CancelClass(CLASS_ID);
                }

                result = "Class Canceled Successfully";
            }
            catch (Exception e)
            {
                result = "That's an error: " + e.Message;
            }
            return result;
        }

        public string ClearSchedule()
        {
            var result = "";
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    //db.ClearSchedule();
                }

                result = "Schedule Has Been Cleared";

            }
            catch (Exception e)
            {
                result = "That's an error: " + e.Message;
            }

            return result;
        }

        public string GenerateCode()
        {
            string code = Guid.NewGuid().ToString();
            code = code.Substring(0, 5).ToUpper();

            ApplicationUser c = new ApplicationUser();
            c.SetAdminTempContract(code);

            return code;
        }
        public bool ReadTempCode(string code)
        {
            bool match;
            ApplicationUser c = new ApplicationUser();
            match = c.ReadAdminTempContract(code);

            return match;
        }

    }
    #endregion

    #region Registrations
    public class RegistrationAdministration
    {
        public SelectList ScheduleDates { get; set; }
        public List<RegistrationCount> CurrentRegCounts { get; set; }
        public List<Registration> StudentsByClass {get; set;}

        public List<GetRegistrationResult> StudentRegistration { get; set; }

        public List<GetPaymentResult> RegistrationPayment { get; set; }

        public SelectList Get_ScheduleDates(string value)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (vmts_dataDataContext db = new vmts_dataDataContext())
            {
                var sql = (from c in db.Class_Schedule_views
                           orderby c.CLASS_START_DATE
                           where c.OPEN_SEATS >=1
                           select (c)).ToList();
                foreach (var r in sql)
                {
                    list.Add(new SelectListItem { Value = r.CLASS_ID.ToString(), Text = r.ClassDay });
                }
            }

            return new SelectList(list, "Value", "Text", value);
        }


        public List<RegistrationCount> Get_CurrentRegCounts()
        {
            var l = new List<RegistrationCount>();
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = (from c in db.RegistrationCounts
                               orderby c.CLASS_START_DATE
                               select (c)).ToList();
                    foreach (var r in sql)
                    {
                        l.Add(new RegistrationCount
                        {
                            CLASS_ID = r.CLASS_ID,
                            ClassDay = r.ClassDay,
                            REGISTRATIONS = r.REGISTRATIONS,
                            OPEN_SEATS = r.OPEN_SEATS,
                            CLASS_START_DATE = r.CLASS_START_DATE,
                            CLASS_TYPE = r.CLASS_TYPE
                        });
                    }
                }
            }
            catch (Exception e)
            {
                l.Add(new RegistrationCount { CLASS_ID = 0, ClassDay = e.Message });
            }

            return l;
        }

        public List<Registration> Get_StudentsByClass(Int32 id)
        {
            var l = new List<Registration>();

            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = (from c in db.Registrations
                               where c.CLASS_ID == id
                               select (c)).ToList();
                    foreach (var r in sql)
                    {
                        l.Add(new Registration
                        {
                            CLASS_ID = r.CLASS_ID,
                            NAME = r.NAME,
                            AGE = r.AGE,
                            EMAIL = r.EMAIL,
                            CLASS_DAY = r.CLASS_DAY,
                            PHONE = r.PHONE,
                            STUDENT = r.STUDENT,
                            RECEIPT = r.RECEIPT,
                            REG_DATE = r.REG_DATE,
                            CONFIRMED = r.CONFIRMED,
                            CONFIRMED_DATE = r.CONFIRMED_DATE,
                            PROCESSED = r.PROCESSED
                            
                        });
                    }
                }
            }
            catch (Exception e)
            {
                l.Add(new Registration { CLASS_ID = 0, NAME = e.Message });
            }

            return l;
        }

        public List<GetRegistrationResult> Get_StudentRegistration(Int64 receipt)
        {
            var l = new List<GetRegistrationResult>();
            /* Fields Required
               First Name
             * Middle Name
             * Last Name
             * Suffix
             * Phone
             * Gender
             * Date of Birth
             * Race
             * Email
             * Address 1
             * Address 2
             * City
             * State
             * Zip
             * DL State
             * DL Number
             */

            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = (from c in db.GetRegistration(receipt)
                               select (c)).ToList();
                    foreach (var r in sql)
                    {
                        l.Add(new GetRegistrationResult
                        {
                            CLASS_ID = r.CLASS_ID,
                            CLASS_DAY = r.CLASS_DAY,
                            CLASS_TYPE = r.CLASS_TYPE,
                            FIRSTNAME = r.FIRSTNAME,
                            MIDDLENAME = r.MIDDLENAME,
                            LASTNAME = r.LASTNAME,
                            SUFFIX = r.SUFFIX,
                            PHONE = r.PHONE,
                            GENDER = r.GENDER,
                            DOB = r.DOB,
                            RACE = r.RACE,
                            EMAIL = r.EMAIL,
                            ADDRESS1 = r.ADDRESS1,
                            ADDRESS2 = r.ADDRESS2,
                            CITY = r.CITY,
                            STATE = r.STATE,
                            STATE_NAME = r.STATE_NAME,
                            ZIP = r.ZIP,
                            DL_ST = r.DL_ST,
                            DL_NUM = r.DL_NUM,
                            EVAL = r.EVAL,
                            MOTOR_YR = r.MOTOR_YR,
                            MOTOR_MK = r.MOTOR_MK,
                            MOTOR_MD = r.MOTOR_MD,
                            STUDENT_ID = r.STUDENT_ID,
                            RECEIPT = r.RECEIPT,
                            REG_DATE = r.REG_DATE,
                            CONFIRMED = r.CONFIRMED,
                            CONFIRMED_DATE = r.CONFIRMED_DATE

                        });
                    }
                }
            }
            catch (Exception e)
            {
                l.Add(new GetRegistrationResult { CLASS_ID = 0, FIRSTNAME = e.Message });
            }

            return l;
        }

       [RoleName]
        public List<GetPaymentResult> Get_RegistrationPayment(long id)
        {
            var l = new List<GetPaymentResult>();
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = (from c in db.GetPayment(id)
                               select (c)).ToList();
                    foreach (var r in sql)
                    {
                        l.Add(new GetPaymentResult
                        {
                            RECEIPT = r.RECEIPT,
                            PMT_TYPE = r.PMT_TYPE,
                            CARDNAME = r.CARDNAME,
                            CARDNUMBER = r.CARDNUMBER,
                            CARD_EXP = r.CARD_EXP,
                            CARD_EXP_YR = r.CARD_EXP_YR,
                            CVV = r.CVV,
                            PROCESSED = r.PROCESSED,
                            TOTAL = r.TOTAL

                        });
                    }
                }
            }
            catch (Exception e)
            {
                l.Add(new GetPaymentResult { RECEIPT = 0, CARDNAME = e.Message });
            }

            return l;
        }

       public string ProcessPayment(Int64 RECEIPT)
       {
           CommunicationModel cm = new CommunicationModel();

           string msg = "";
           try
           {
               /* SEND PAYMENT CONFIRMATION MESSAGE */
               msg = cm.PaymentConfirmation(RECEIPT);

               if (msg == "Sent")
               {
                   using (vmts_dataDataContext db = new vmts_dataDataContext())
                   {
                       db.ProcessPayment(RECEIPT);
                   }
               msg = "Success";
               }
           }
           catch
           {
               msg = "Fail";
           }
           return msg;
       }

        [RoleName]
        public string CancelReg(Int64 RECEIPT)
        {
            string msg = "";
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    db.CancelRegistration(RECEIPT);
                }
                msg = "Success";
            }
            catch
            {
                msg = "Fail";
            }
            return msg;
        }

        [RoleName]
        // STUDENT CHANGED CLASSES TO A NEW DATE
        public string ChangeClass(Int64 RECEIPT, Int32 OLD, Int32 NEW)
        {
            string msg = "";
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    db.SwitchClass(RECEIPT,OLD,NEW);
                }
                msg = "Success";
            }
            catch
            {
                msg = "Fail";
            }
            return msg;
        }


    }
    #endregion

    #region Carousel
    public class Carousel
    {
    }
#endregion

#region Sponsors
    public class SponsorsPage
    {
        public string Name;
        public string WebSite;
        public string Logo;
    }
    public class Sponsors{
        string jsonFileSponsors = ConfigurationManager.AppSettings["SponsorsJSONFile"].ToString();

        public List<SponsorsPage> CurrentSponsors { get; set; }
        public List<SponsorsPage> RemoveSponsor { get; set; }
        public List<SponsorsPage> GetCurrentSponsors()
        {
            var l = new List<SponsorsPage>();
            try
            {

                JObject s = JObject.Parse(File.ReadAllText(jsonFileSponsors));

                JArray promoCodes = (JArray)s["Sponsors"];
                foreach (var c in promoCodes)
                {
                    l.Add(new SponsorsPage
                    {
                        Name = (string)c["Name"],
                        WebSite = (string)c["WebSite"],
                        Logo = (string)c["Logo"],
                    });

                }
            }
            catch (Exception e)
            {
                l.Add(new SponsorsPage { Name = e.Message });
            }
            return l;

        }

    }
#endregion


#region PromoCodes

    public class PROMO_CODE
    {
        public string PromoCode;
        public DateTime StartDate;
        public DateTime EndDate;
        public string Value;
        public bool Valid;

    }
    public class PromoCodes
    {
        string jsonFilePromoCodes = ConfigurationManager.AppSettings["PromoCodeJSONFile"].ToString();
        public List<PROMO_CODE> AllCodes { get; set; }
        public List<PROMO_CODE> GetAllPromoCodes()
        {
            var AllCodes = new List<PROMO_CODE>();
            try
            {

                JObject codes = JObject.Parse(File.ReadAllText(jsonFilePromoCodes));

                JArray promoCodes = (JArray)codes["PromoCodes"];
                foreach (var c in promoCodes)
                {
                    AllCodes.Add(new PROMO_CODE
                    {
                        PromoCode = (string)c["PromoCode"],
                        StartDate = (DateTime)c["StartDate"],
                        EndDate = (DateTime)c["EndDate"],
                        Value = (string)c["Value"],
                        Valid = (bool)c["Valid"]
                    });

                }
            }
            catch (Exception e)
            {
                AllCodes.Add(new PROMO_CODE { PromoCode = e.Message });
            }

            return AllCodes;

        }


    }
    #endregion
}