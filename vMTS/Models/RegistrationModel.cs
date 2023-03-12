using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;

namespace vMTS.Models
{
    public class NameSuffix
    {
        public List<SelectListItem> list { get; set; }
        public SelectList SuffixSelectList()
        {
            list = new List<SelectListItem>();
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = (from a in db.NAME_SUFFIXes
                               select (a)).ToList();

                    foreach (var d in sql)
                    {
                        list.Add(new SelectListItem
                        {
                            Value = d.SUFFIX_ABBR.ToString(),
                            Text = d.SUFFIX

                        });
                    }
                }
            }
            catch //(Exception e)
            {
                list.Add(new SelectListItem { Value = "", Text = "Select" });
            }
            return new SelectList(list, "Value", "Text");
        }
    }
    public class RaceLookup
        {
        public List<SelectListItem> list { get; set; }
        public SelectList RaceSelectList()
            {
            list = new List<SelectListItem>();
                try
                {
                    using (vmts_dataDataContext db = new vmts_dataDataContext())
                    {
                        var sql = (from a in db.RACE_LKPs
                                   orderby a.sequence
                                   select (a)).ToList();

                        foreach (var d in sql)
                        {
                            list.Add(new SelectListItem
                            {
                                Value = d.R_ID.ToString(),
                                Text = d.RACE_DESC

                            });
                        }
                    }
                }
                catch //(Exception e)
                {
                    list.Add(new SelectListItem { Value = "8", Text = "Select" });
                }
                return new SelectList(list, "Value", "Text");
            }
        }
        public class StateList
        {
        public List<SelectListItem> list { get; set; }
        public SelectList StateSelectList()
            {
            list = new List<SelectListItem>();
                try
                {
                    list.Add(new SelectListItem { Value = "AL", Text = "Alabama" });
                    list.Add(new SelectListItem { Value = "AK", Text = "Alaska" });
                    list.Add(new SelectListItem { Value = "AZ", Text = "Arizona" });
                    list.Add(new SelectListItem { Value = "AR", Text = "Arkansas" });
                    list.Add(new SelectListItem { Value = "CA", Text = "California" });
                    list.Add(new SelectListItem { Value = "CO", Text = "Colorado" });
                    list.Add(new SelectListItem { Value = "CT", Text = "Connecticut" });
                    list.Add(new SelectListItem { Value = "DE", Text = "Delaware" });
                    list.Add(new SelectListItem { Value = "DC", Text = "District Of Columbia" });
                    list.Add(new SelectListItem { Value = "FL", Text = "Florida" });
                    list.Add(new SelectListItem { Value = "GA", Text = "Georgia" });
                    list.Add(new SelectListItem { Value = "HI", Text = "Hawaii" });
                    list.Add(new SelectListItem { Value = "ID", Text = "Idaho" });
                    list.Add(new SelectListItem { Value = "IL", Text = "Illinois" });
                    list.Add(new SelectListItem { Value = "IN", Text = "Indiana" });
                    list.Add(new SelectListItem { Value = "IA", Text = "Iowa" });
                    list.Add(new SelectListItem { Value = "KS", Text = "Kansas" });
                    list.Add(new SelectListItem { Value = "KY", Text = "Kentucky" });
                    list.Add(new SelectListItem { Value = "LA", Text = "Louisiana" });
                    list.Add(new SelectListItem { Value = "ME", Text = "Maine" });
                    list.Add(new SelectListItem { Value = "MD", Text = "Maryland" });
                    list.Add(new SelectListItem { Value = "MA", Text = "Massachusetts" });
                    list.Add(new SelectListItem { Value = "MI", Text = "Michigan" });
                    list.Add(new SelectListItem { Value = "MN", Text = "Minnesota" });
                    list.Add(new SelectListItem { Value = "MS", Text = "Mississippi" });
                    list.Add(new SelectListItem { Value = "MO", Text = "Missouri" });
                    list.Add(new SelectListItem { Value = "MT", Text = "Montana" });
                    list.Add(new SelectListItem { Value = "NE", Text = "Nebraska" });
                    list.Add(new SelectListItem { Value = "NV", Text = "Nevada" });
                    list.Add(new SelectListItem { Value = "NH", Text = "New Hampshire" });
                    list.Add(new SelectListItem { Value = "NJ", Text = "New Jersey" });
                    list.Add(new SelectListItem { Value = "NM", Text = "New Mexico" });
                    list.Add(new SelectListItem { Value = "NY", Text = "New York" });
                    list.Add(new SelectListItem { Value = "NC", Text = "North Carolina" });
                    list.Add(new SelectListItem { Value = "ND", Text = "North Dakota" });
                    list.Add(new SelectListItem { Value = "OH", Text = "Ohio" });
                    list.Add(new SelectListItem { Value = "OK", Text = "Oklahoma" });
                    list.Add(new SelectListItem { Value = "OR", Text = "Oregon" });
                    list.Add(new SelectListItem { Value = "PA", Text = "Pennsylvania" });
                    list.Add(new SelectListItem { Value = "RI", Text = "Rhode Island" });
                    list.Add(new SelectListItem { Value = "SC", Text = "South Carolina" });
                    list.Add(new SelectListItem { Value = "SD", Text = "South Dakota" });
                    list.Add(new SelectListItem { Value = "TN", Text = "Tennessee" });
                    list.Add(new SelectListItem { Value = "TX", Text = "Texas" });
                    list.Add(new SelectListItem { Value = "UT", Text = "Utah" });
                    list.Add(new SelectListItem { Value = "VT", Text = "Vermont" });
                    list.Add(new SelectListItem { Value = "VA", Text = "Virginia" });
                    list.Add(new SelectListItem { Value = "WA", Text = "Washington" });
                    list.Add(new SelectListItem { Value = "WV", Text = "West Virginia" });
                    list.Add(new SelectListItem { Value = "WI", Text = "Wisconsin" });
                    list.Add(new SelectListItem { Value = "WY", Text = "Wyoming" });
                }
                catch //(Exception e)
                {
                    list.Add(new SelectListItem { Value = "", Text = "Select" });
                }
                return new SelectList(list, "Value", "Text");
            }
        }

    public class RegistrationModel
    {
        public class PaymentConfirm
        {
            public string PMT_TYPE { get; set; }
            public string CARDNAME { get; set; }
            public string CARDNUMBER { get; set; }
            public string CARD_EXP { get; set; }
            public string CARD_EXP_YR { get; set; }
            public string CVV { get; set; }
            public string PROMOCODE { get; set; }

            public decimal? CLASS_FEE { get; set; }
            public decimal? TOTAL { get; set; }
            public decimal? PROMO_VALUE { get; set; }
        }
        public SelectList NameSuffix { get; set; }

        public SelectList RaceCodes { get; set; }
        public SelectList States { get; set; }


        public List<Registration> Confirmation { get; set; }
        public List<PaymentConfirm> RegistrationPayment { get; set; }

        public List<Registration> GetConfirmation(Int64 id) /* the id is the receipt id for the registration*/
        {
            var l = new List<Registration>();
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = (from c in db.Registrations
                               where c.RECEIPT == id
                               select (c)).ToList();
                    foreach (var r in sql)
                    {
                        l.Add(new Registration
                        {
                            RECEIPT = r.RECEIPT,
                            CLASS_ID = r.CLASS_ID,
                            CLASS_TYPE = r.CLASS_TYPE,
                            CLASS_DAY = r.CLASS_DAY,
                            CLASS_START_DATE = r.CLASS_START_DATE,
                            CLASS_START_TIME = r.CLASS_START_TIME,
                            CLASS_END_DATE = r.CLASS_END_DATE,
                            CLASS_END_TIME = r.CLASS_END_TIME,
                            CLASS_LOCATION = r.CLASS_LOCATION,
                            COURSE_NUMBER = r.COURSE_NUMBER,
                            NAME = r.NAME,
                            SUFFIX = r.SUFFIX,
                            ADDRESS = r.ADDRESS,
                            CITY = r.CITY,
                            STATE = r.STATE,
                            ZIP = r.ZIP,
                            REG_DATE = r.REG_DATE,
                            DOB = r.DOB,
                            AGE = r.AGE,
                            WAIVER = r.WAIVER,
                            EMAIL = r.EMAIL,
                            PHONE = r.PHONE,
                            VERBAL_FLAG = r.VERBAL_FLAG,
                            CONFIRMED = r.CONFIRMED,
                            CONFIRMED_DATE = r.CONFIRMED_DATE,
                            EVAL = r.EVAL
                        });
                    }
                }
            }
            catch (Exception e)
            {
                l.Add(new Registration { RECEIPT = id, NAME = e.Message });
            }
            return l;

        }

        public List<PaymentConfirm> GetRegistrationPayment(Int64 id) /* id is equal to the receipt id for the registration*/
        {
            var l = new List<PaymentConfirm>();
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    var sql = (from c in db.GetPayment(id)
                               select (c)).ToList();
                    foreach (var r in sql)
                    {
                        l.Add(new PaymentConfirm
                        {
                            PMT_TYPE = r.PMT_TYPE,
                            CARDNAME = r.CARDNAME,
                            CARDNUMBER = r.STUFF_CARDNUMBER,
                            CARD_EXP = r.STUFF_CARD_EXP,
                            CARD_EXP_YR = r.STUFF_CARD_EXP_YR,
                            CVV = r.STUFF_CVV,
                            PROMOCODE = r.PROMOCODE,
                            CLASS_FEE = r.CLASS_FEE,
                            TOTAL = r.TOTAL,
                            PROMO_VALUE = r.PROMO_VALUE
                            
                        });
                    }
                }
            }
            catch (Exception e)
            {
                l.Add(new PaymentConfirm { PMT_TYPE = e.Message });
            }
            return l;

        }

        public string RegistrationConfirm(Int64 RECEIPT)
        {
            string msg = "";
            try
            {
                using (vmts_dataDataContext db = new vmts_dataDataContext())
                {
                    db.ConfirmRegistration(RECEIPT);
                }
                msg = "Success";
            }
            catch
            {
                msg = "Fail";
            }
            return msg;
        }


        //public string VerifyPromoCode(string code)
        //{
        //    string msg;
        //    using (vmts_dataDataContext db = new vmts_dataDataContext())
        //    {
        //        msg = (from c in db.PROMO_CODEs
        //               where c.PROMO_CODE1 == code
        //               select (c.VALID).ToString()).FirstOrDefault();
        //    }
        //    if (msg != "True") { msg = "false"; }

        //    return msg;
        //}

        public Int32 VerifyClassOpenings(Int32 id)
        {
            Int32 msg = 0;
            using (vmts_dataDataContext db = new vmts_dataDataContext())
            {
               var os = (from c in db.Class_Schedule_views
                       where c.CLASS_ID == id
                       select c.OPEN_SEATS).FirstOrDefault();
                msg = Convert.ToInt32(os);
            }

            return msg;
        }
        public string ReplaceCardNum(string CardNumber)
        {
             string newNum = "";
           if (CardNumber != null) { 
            string p = "[0-9]{12}0+";
            newNum = Regex.Replace(CardNumber, p, "X");
            }
            return newNum;
        }
    }

    public class CustomErrors{
        public void NoResponse()
        {
            throw new HttpException(204, "No Response");
        }

        public void BadRequest()
        {
            throw new HttpException(400, "Bad Request");
        }

    }

public class AddressValidation{

    public XmlDocument ValidateAddress(string Address, string City, string State, string Zip)
    {
        //string APIresponse="";
        string addressRequest = CreateRequest(Address, City,State, Zip);
        XmlDocument addressResponse = MakeRequest(addressRequest);
        
        //APIresponse = ProcessResponse(addressResponse);

        //return ProcessResponse(addressResponse);
        return addressResponse;
    }
    public static string CreateRequest(string Ad1,string City, string State, string Zip)
    {
        string USERID = "254LOGIT1779";
        string XMLRequest = "http://production.shippingapis.com/ShippingAPITest.dll?API=Verify&XML=";
        XMLRequest += "<AddressValidateRequest USERID='" + USERID + "'>";
        XMLRequest += "<Address ID='0'>";
        XMLRequest += "<FirmName />";
        XMLRequest += "<Address1 />";
        XMLRequest += "<Address2>" + Ad1 + "</Address2>";
        XMLRequest += "<City></City>";
        XMLRequest += "<State></State>";
        XMLRequest += "<Zip5>" + Zip + "</Zip5>";
        XMLRequest += "<Zip4></Zip4>"; 
        XMLRequest += "</Address>"; 
        XMLRequest += "</AddressValidateRequest>";

        return XMLRequest;
    }

    public static XmlDocument MakeRequest(string requestURL)
    {
        HttpWebRequest request = WebRequest.Create(requestURL) as HttpWebRequest;
        HttpWebResponse response = request.GetResponse() as HttpWebResponse;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(response.GetResponseStream());

        return xmlDoc;

    }

    public string ProcessResponse(XmlDocument xmlAddress)
    {
        XmlNamespaceManager XMLns = new XmlNamespaceManager(xmlAddress.NameTable);
        XMLns.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");
    
        //Get the valid address
        string address;
        //XmlNodeList addressElements = xmlAddress.SelectNodes("//rest:AddressValidateResponse", XMLns);
        //XmlNodeList errorElements = xmlAddress.SelectNodes("//rest:AddressValidateResponse/rest:Error", XMLns);
        string errorNum = xmlAddress.GetElementsByTagName("Number")[0].InnerText;

       if(errorNum!= "")
        {
           string d = xmlAddress.GetElementsByTagName("Description")[0].InnerText.Trim();
           address = errorNum + "," + d;
        }
       else
       {
           string a = xmlAddress.GetElementsByTagName("Address2")[0].InnerText;
           string c = xmlAddress.GetElementsByTagName("City")[0].InnerText;
           string s = xmlAddress.GetElementsByTagName("State")[0].InnerText;
           string z = xmlAddress.GetElementsByTagName("Zip5")[0].InnerText;

           address = a.Trim() + "," + c.Trim() + "," + s.Trim() + "," + z.Trim();
       }
       return address;    
    }
}

}