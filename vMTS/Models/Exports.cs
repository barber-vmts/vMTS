using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/* FOR THE CSV EXPORT */
using System.Data;
using System.Configuration;

/* FOR THE EXCEL EXPORT 
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;*/
using System.Reflection;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace vMTS.Models
{
    public class Exports
    {
        RegistrationAdministration ra = new RegistrationAdministration();

        public class MREP
        {
            public string FirstName {get; set;}
            public string MiddleName{get; set;}
            public string LastName{get; set;}
            public string Suffix{get; set;}
            public string Nickname{get; set;}
            public string ContactType{get; set;}
            public string Phone{get; set;}
            public string Gender{get; set;}
            public string DateofBirth{get; set;}
            public string Race{get; set;}
            public string Email{get; set;}
            public string TShirtSize{get; set;}
            //public string LicensingInstructorNumber{get; set;}
            public string Street1{get; set;}
            public string Street2{get; set;}
            public string City{get; set;}
            public string State{get; set;}
            public string ZIP{get; set;}
            public string DriverLicenseState{get; set;}
            public string DriverLicenseNumber{get; set;}
            //public string SocialSecurityNumber{get; set;}
            //public string MSFnumber{get; set;}
            //public string Owner{get; set;}
            public string ClassDay { get; set; }
        }

        public void CreateCSV(Int64 r)
        {
             // GET THE REGISTRATION DATA
            ra.StudentRegistration = ra.Get_StudentRegistration(r);
            var list = new List<MREP>();

            foreach (var l in ra.StudentRegistration)
            {
                list.Add(new MREP {
                    FirstName = l.FIRSTNAME,
                    MiddleName = l.MIDDLENAME,
                    LastName = l.LASTNAME,
                    Suffix = l.SUFFIX,
                    Nickname = "",
                    ContactType = "Student",
                    Phone = l.PHONE,
                    Gender = l.GENDER,
                    DateofBirth = l.DOB,
                    Race = l.RACE,
                    Email = l.EMAIL,
                    TShirtSize = "",
                    //LicensingInstructorNumber = "",
                    Street1 = l.ADDRESS1,
                    Street2 = l.ADDRESS2,
                    City = l.CITY,
                    State = l.STATE,
                    ZIP = l.ZIP,
                    DriverLicenseState = l.DL_ST,
                    DriverLicenseNumber = l.DL_NUM,
                    //SocialSecurityNumber = "",
                    //MSFnumber = "",
                    //Owner = ""
                });
            }
            
            //Build the CSV file data as a Comma separated string.
            var csv = new StringBuilder();
          
            //Get the properties for type T for the headers
            PropertyInfo[] propInfos = typeof(MREP).GetProperties();
            string fileName = r + ".csv";
            string pathRoot = @"E:/web/learntor/Content/RegistrationFiles/";
            //string pathRoot = @"E:\LogiTeks\vMTS\Documentation\";
            
            string columns;

            if (list.Count > 0) {
                columns = @"First Name,Middle Name,Last Name,Suffix,Nickname,Contact Type,Phone,Gender,Date of Birth,Race,Email,T-Shirt Size,Address1,Address2,City,State,ZIP,Driver License State,Driver License Number";
                csv.Append(columns);
                csv.Append(Environment.NewLine);

                for (int i = 0; i <= list.Count - 1;  i++ )
                {
                    MREP item = list[i];

                    for (int j = 0; j <= propInfos.Length - 1; j++) { 
                    
                        object o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item,null);
                        string value = "";
                        if (o != null)
                        {
                            value = o.ToString();
                        }
                        else
                        {
                            value = "";
                        }
                            csv.Append(value);
                            if (j < propInfos.Length - 1)
                            {
                                csv.Append(",");
                            }
                    }
                } 
           }

            File.WriteAllText(pathRoot + fileName, csv.ToString());
        }

        [RoleName]
        public List<MREP> CreateClassCSV(Int32 COURSE_ID)
        {
            var list = new List<MREP>();
            List<Registration> StudentsByClass = new List<Registration>();
            StudentsByClass = ra.Get_StudentsByClass(COURSE_ID);

            foreach (var s in StudentsByClass)
            {
                // GET THE STUDENT REGISTRATION DATA
                Int64 r = s.RECEIPT;
                ra.StudentRegistration = ra.Get_StudentRegistration(r);

                foreach (var l in ra.StudentRegistration)
                {
                    list.Add(new MREP
                    {
                        FirstName = l.FIRSTNAME,
                        MiddleName = l.MIDDLENAME,
                        LastName = l.LASTNAME,
                        Suffix = l.SUFFIX,
                        Nickname = "",
                        ContactType = "Student",
                        Phone = l.PHONE,
                        Gender = l.GENDER,
                        DateofBirth = l.DOB,
                        Race = l.RACE,
                        Email = l.EMAIL,
                        TShirtSize = "",
                        //LicensingInstructorNumber = "",
                        Street1 = l.ADDRESS1,
                        Street2 = l.ADDRESS2,
                        City = l.CITY,
                        State = l.STATE,
                        ZIP = l.ZIP,
                        DriverLicenseState = l.DL_ST,
                        DriverLicenseNumber = l.DL_NUM,
                        ClassDay = l.CLASS_DAY
                        //,SocialSecurityNumber = "",
                        //MSFnumber = "",
                        //Owner = ""
                    });
                }
            }

            return list;
        }
    }
}