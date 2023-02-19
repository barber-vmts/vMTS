using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace vMTS.Models
{
    public class CourseDescriptions
    {
        public string CourseName;
        public JArray Description;
        public string MinRiders;
        public JArray Requirements;
        public string Evaluation;
        public string FeeStructure;
        public string Cost;
    }

    public class Instructors
    {
        public string Name;
        public JArray Bio;
        public string Title;
        public string Email;
        public string Photo;

    }
    public class Sponsor
    {
        public string Name;
        public string WebSite;
        public string Logo;

    }

    public class CarouselImgs
    {
        public string ImgName;
        public string ImgCaption;
        public Int32 Sequence;

    }

    public class InstructorClasses
    {
        public string ClassDates;
        public string PrimaryInstructor;
        public string SecondaryInstructor;

    }


    public class GeneralModel
    {
        string jsonFileCourses = ConfigurationManager.AppSettings["CourseJSONFile"].ToString();
        string jsonFileInstructors = ConfigurationManager.AppSettings["InstructorsJSONFile"].ToString();
        string jsonFileSponsors = ConfigurationManager.AppSettings["SponsorsJSONFile"].ToString();
        string jsonFileCarousel = ConfigurationManager.AppSettings["CarouselJSONFile"].ToString();
        string jsonFileInstructorClasses = ConfigurationManager.AppSettings["InstructorClassesJSONFile"].ToString();
        string jsonFilePromoCodes = ConfigurationManager.AppSettings["PromoCodeJSONFile"].ToString();

        public List<CarouselImgs> GetCarouselImages { get; set; }

        public List<CourseDescriptions> GetCourseDescriptions()
        {
            var l = new List<CourseDescriptions>();
            try
            {
            JObject courses = JObject.Parse(File.ReadAllText(HostingEnvironment.MapPath(jsonFileCourses)));

            JArray riderCourses = (JArray)courses["Courses"];
            foreach (var c in riderCourses)
            {
                l.Add(new CourseDescriptions
                {
                    CourseName = (string)c["CourseName"],
                    Description = (JArray)c["Description"],
                    MinRiders = (string)c["MinRiders"],
                    Requirements = (JArray)c["Requirements"],
                    Evaluation = (string)c["Evaluation"],
                    FeeStructure = (string)c["FeeStructure"],
                    Cost = (string)c["Cost"]
                });

            }
            
            }
            catch
            {
                l.Add(new CourseDescriptions
                {
                    CourseName = "",
                    Description = (JArray)"",
                    MinRiders = "0",
                    Requirements = null
                });
            }
            return l;
        }
        public List<Sponsor> GetAllSponsors()
        {
            var l = new List<Sponsor>();
            try{
                JObject sponsors = JObject.Parse(File.ReadAllText(HostingEnvironment.MapPath(jsonFileSponsors)));

                JArray riderCoaches = (JArray)sponsors["Sponsors"];
                foreach (var c in riderCoaches)
                {
                    l.Add(new Sponsor
                    {
                        Name = (string)c["Name"],
                        WebSite = (string)c["WebSite"],
                        Logo = (string)c["Logo"],
                    });

                }
            }
            catch (Exception e)
            {
                CommunicationModel C = new CommunicationModel();
                C.SendErrorEmail(e.Message + Environment.NewLine + "Sponsors JSON file:", "GetAllSponsors");

                C.LogErrorToDB("GetAllSponsors", e.Message, e.StackTrace);
            }

            return l; 
        }

        public List<Instructors> GetAllInstructors()
        {
            var l = new List<Instructors>();
            try
            {
                JObject instructors = JObject.Parse(File.ReadAllText(HostingEnvironment.MapPath(jsonFileInstructors)));

                JArray riderCoaches = (JArray)instructors["Instructors"];
                foreach (var c in riderCoaches)
                {
                    l.Add(new Instructors
                    {
                        Name = (string)c["Name"],
                        Bio = (JArray)c["Bio"],
                        Title = (string)c["Title"],
                        Email = (string)c["Email"],
                        Photo = (string)c["Photo"]
                    });

                }
            }
            catch (Exception e)
            {
                CommunicationModel C = new CommunicationModel();
                C.SendErrorEmail(e.Message + Environment.NewLine + "Instructors JSON file:", "GetAllInstructors");

                C.LogErrorToDB("GetAllInstructors", e.Message, e.StackTrace);
            }

            //return l.OrderBy(x => x.Name).ToList();
            return l;
        }

        public List<InstructorClasses> GetInstructorClass()
        {
            var l = new List<InstructorClasses>();
            try
            {
                JObject instructors = JObject.Parse(File.ReadAllText(HostingEnvironment.MapPath(jsonFileInstructorClasses)));

                JArray instClasses = (JArray)instructors["InstructorsClasses"];
                foreach (var c in instClasses)
                {
                    l.Add(new InstructorClasses
                    {
                        ClassDates = (string)c["ClassDates"],
                        PrimaryInstructor = (string)c["PrimaryInstructor"],
                        SecondaryInstructor = (string)c["SecondaryInstructor"]
                    });

                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                CommunicationModel C = new CommunicationModel();
                C.SendErrorEmail(e.Message + Environment.NewLine + "Instructor Class JSON file:", "GetInstructorClass");

                C.LogErrorToDB("GetInstructorClass", e.Message, e.StackTrace);
            }

            //return l.OrderBy(x => x.Name).ToList();
            return l;
        }
        public List<CarouselImgs> GetAllImages()
        {
            var l = new List<CarouselImgs>();
            try
            {
                JObject carousel = JObject.Parse(File.ReadAllText(HostingEnvironment.MapPath(jsonFileCarousel)));

                JArray carImages = (JArray)carousel["Carousel"];
                foreach (var c in carImages)
                {
                    l.Add(new CarouselImgs
                    {
                        ImgName = (string)c["Name"],
                        ImgCaption = (string)c["Caption"],
                        Sequence = (Int32)c["Sequence"]
                    });

                }
            }
            catch (Exception e)
            {
                l.Add(new CarouselImgs { ImgCaption = e.Message });

                CommunicationModel C = new CommunicationModel();
                C.SendErrorEmail(e.Message + Environment.NewLine + "Carousel Images:", "GetAllImages");

                C.LogErrorToDB("GetAllImages", e.Message, e.StackTrace);

            }

            return l.OrderBy(x => x.Sequence).ToList();
        }


       public string VerifyPromoCode(string code)
        {
            string msg;
            try
            {
            JObject codes = JObject.Parse(HostingEnvironment.MapPath(File.ReadAllText(jsonFilePromoCodes)));

            msg = (from c in codes["PromoCodes"]
                   where c["PromoCode"].ToString() == code
                   select (c["Valid"]).ToString()).FirstOrDefault();

            if (msg != "True") { msg = "false"; }

            }
            catch (Exception e)
            {
                msg = "This feature is not functioning properly, please contact a vMTS representative for more information";

                CommunicationModel C = new CommunicationModel();
                C.SendErrorEmail(e.Message + Environment.NewLine + "Promo Code Value: " + code, "VerifyPromoCode");

                C.LogErrorToDB("VerifyPromoCode", e.Message, e.StackTrace);
            }

            return msg;
        }

        public decimal GetPromoCodeValue(string code)
        {

            decimal value;
            string jsonValue;
            try
            {
                JObject codes = JObject.Parse(HostingEnvironment.MapPath(File.ReadAllText(jsonFilePromoCodes)));

                jsonValue = (from c in codes["PromoCodes"]
                             where c["PromoCode"].ToString() == code && c["Valid"].ToString() == "True"
                             select (c["Value"]).ToString()).FirstOrDefault();

                value = Convert.ToDecimal(jsonValue);
            }
            catch
            {
                value = 0;
            }


            return value;
        }

    }
}