using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace vMTS.Models
{
    public class CommunicationModel
    {
        RegistrationModel r = new RegistrationModel();

        string devmail = "lloydleebarber@gmail.com";
        string adminmail = "Melanie.Barber124@gmail.com";
        string owneremail = "Steve.Barber@comcast.net";
        string smtp = "mail.learntoridetn.com";
        string postmaster = "postmaster@learntoridetn.com";
        string pass = "vmts2013!";
        public void SendRegistrationMessage(Int64 id)
        {
            var regList = new List<Registration>();

            regList = r.GetConfirmation(id);

            var msg = "";
            var body = "";//<h1>THIS IS ONLY A TEST</h1>
            try
            {
                body += "<h1>Thank You for Your Registration</h1>";
                body += "<table cellpadding='10'><tr><td>Registration #:</td><td>" + id + "</td></tr>";
                body += "<tr><td>Registration Date:</td><td>" + regList.FirstOrDefault().REG_DATE + "</td></tr></table>";
                body += "<table><tr><td>Course Type:</td><td>" + regList.FirstOrDefault().CLASS_TYPE + "</td></tr>";
                body += "<tr><td>Class Date:</td><td>" + regList.FirstOrDefault().CLASS_DAY + "</td></tr>";
                body += "<tr><td>Participant Name:</td><td>" + regList.FirstOrDefault().NAME + "</td></tr>";
                body += "<tr><td>Participant Phone:</td><td>" + regList.FirstOrDefault().PHONE + "</td></tr></table>";

                if (regList.FirstOrDefault().AGE <= 17)
                {
                    body += "<h1>Because the participant is under 18, a parent or legal guardian will need to be present to sign a waiver.</h1>";

                    if (regList.FirstOrDefault().AGE == 14)
                    {
                        body += "<h3>The participant is " + regList.FirstOrDefault().AGE + " years old, therefore he/she will not be able to use the certificate until turning 15 years of age.</h3>";
                    }                    
                    else if (regList.FirstOrDefault().AGE <= 13)
                    {
                        body += "<h3>The participant will not be issued a certificate because he/she is only " + regList.FirstOrDefault().AGE + " years of age.</h3>";
                    }
                }

                body += "<table><th>Class Details</th>";
                body += "<tr><td>Class will begin at " + string.Format("{0:t}",regList.FirstOrDefault().CLASS_START_TIME) + " on " + string.Format("{0:D}",regList.FirstOrDefault().CLASS_START_DATE) + ".  Plan to arrive a little early to complete some paper work before class begins.</td></tr>";
                body += "<tr><td>The address of the training site is 270 E. Main Street, Hendersonville, TN 37075.  Note that GPS will take you to the main entrance, west parking lot; we are located on the other side of the plant in the east parking lot.  Come to the Shipping/Receiving entrance next to Pinnacle Bank, across from Simmons Bank.</td></tr>";

                body += "<tr><td>Riding gear includes:<ul><li>DOT helmet - you must provide your own helmet<br><b>Note:</b> If you do not have a helmet, show this confirmation to the staff at Cycle Gear to receive a 10% discount toward a new helmet.  Cycle Gear is located at Rivergate Station, 1677 Gallatin Pike N,Madison, TN 37115</li>";
                body +="<li>Shatter proof eye wear if using open face helmet</li><li>Long sleeve shirt or jacket must be worn while sitting on the motorcycle and riding.</li><li>Full fingered gloves; cut off gloves not allowed</li><li>Long pants- no holes or exposed skin</li><li>Over the ankle boots with good grip soles</li></ul></td></tr>";
                body += "<tr><td><b>You Will not be allowed to particiapte in the riding exercises and will forfeit your fee without all the gear.</b></td></tr>";

                body += "<tr><td>Please review the Motorcycle Safety Foundation Liability Waiver and Indemnification form by clicking on the link <a href='https://www.learntoridetn.com/images/Liability%20Waiver/MSF-RiderCourseWaiver.pdf' target='_blank'>here</a>. You will sign the form upon your arrival to class.</td></tr>";

                if (regList.FirstOrDefault().CLASS_TYPE == "Basic RiderCourse I")
                {
                    body += "<tr><td>A requirement to successfully complete the Basic Rider Course and receive a Tennessee Department of Safety completion certificate good toward licensing is to complete the online eCourse located in an accompanying email. We will also include a crossword puzzle in this email to help you prepare for the test.</td></tr>";
                    body += "<tr><td>Requires a minumum of 4 riders for class to be held.</td></tr>";

                    body += "<tr><td>Learning to ride a motorcycle is challenging both physically and mentally, but certainly attainable. We ask that you not schedule any activities during the two days of class, including work, parties and late night outings. Get a good nights rest the night before the class and the night following the first day. Successful completion is up to you.</td><tr>";
                }

                if (regList.FirstOrDefault().CLASS_TYPE == "Basic RiderCourse II" || regList.FirstOrDefault().CLASS_TYPE == "Advanced RiderCourse")
                {
                    body += "<tr><td>You must come with a DOT street legal motorcycle with all components in good working order, i.e. tires with good tread, both brakes working, all lights working, and plenty of gas in the tank, no physical damage, etc.</td></tr>";
                    body += "<tr><td>You must provide registration and proof of insurance. If you have a borrowed or rented bike, you must bring consent from the owner or a copy of the rental agreement.</td></tr>";
                    body += "<tr><td>Requires a minumum of 4 riders for class to be held.</td></tr>";
                    body += "<tr><td>Learning to ride a motorcycle is challenging both physically and mentally, but certainly attainable. We ask that you not schedule any activities during the two days of class, including work, parties and late night outings. Get a good nights rest the night before the class and the night following the first day. Successful completion is up to you.</td><tr>";
                }

                if (regList.FirstOrDefault().CLASS_TYPE == "3 Wheel Course")
                {
                    body += "<tr><td>You must come with a DOT street legal trike with all components in good working order, i.e. tires with good tread, both brakes working, clutch properly working, and plenty of gas in the tank, no physical damage, etc.</td></tr>";
                    body += "<tr><td>You must provide registration and proof of insurance. If you have a borrowed or rented trike, you must bring consent from the owner or a copy of the rental agreement.</td></tr>";

                    body += "<tr><td>To help better prepare for class, we suggest reviewing the online study guide at <a href='http://www.msf-usa.org/downloads/3WBRC_Student_Handbook_2010.pdf' target='_blank'>www.msf-usa.org/downloads/3WBRC_Student_Handbook_2010.pdf</a> with study questions beginning on page 51.</td></tr>";
                    body += "<tr><td>Another good source is the MSF 48 question review at <a href='http://www.msf-usa.org/BRCQuiz.aspx#/H3w0CSdF1x/PlnH05KLo1/SfsW80tFCk' target='_blank'>www.msf-usa.org/BRCQuiz.aspx#/H3w0CSdF1x/PlnH05KLo1/SfsW80tFCk</a>.</td></tr>";
                    body += "<tr><td>Learning to ride a 3 wheel motorcycle is challenging both physically and mentally, but certainly attainable. We ask that you not schedule any activities during the two days of class, including work, parties and late night outings. Get a good nights rest the night before the class and the night following the first day. Successful completion is up to you.</td><tr>";
                }

                if (regList.FirstOrDefault().CLASS_TYPE == "Skills Practice and Improvement")
                {
                    body += "<tr><td>Requires a minumum of 10 riders for class to be held.</td></tr>";
                }
                body += "<tr><td>We will take lots of breaks and there will be a lunch break included.  Feel free to bring a lunch and snacks.  We will provide water.</td></tr>";
              
                body += "</table>";

                body += "<table><tr><td><h3>Important Refund/Cancellation Policy</h3></td></tr>";
                body += "<tr><td>NO REFUNDS will be made, except for course cancellations, at least 7 days in advance of the scheduled class date. One reschedule will be permitted if notified at least 24 hours in advance of the class start time. The cost of the reschedule is $100. If a request is not made within the specified time period, the class fee will be forfeited and a new fee will be charged for a later class.</td></tr>";
                body += "<tr><td>Students who are unable to meet the minimum physical requirements in the opinion of the RiderCoach, or students whose behavior pose a hazard to themselves and/or other students will be asked to discontinue the riding portion of the class with NO REFUND GIVEN. They may stay to observe the remainder of the class but will not be certified for completion. Students must successfully complete the entire class (including the written and riding skills evaluation) to receive a completion certificate and MSF card completion card.</tr></td></table>";

                body += "<table>";
                body += "<tr><td>If links in the email do not work, copy and paste them into your browser</td></tr>";
                body += "<tr><td>Do not reply to this email.  If you need assistance, please notify Steve Barber, site administrator, by phone or text to 615.414.9042 or email at <a href='mailto:steve.barber@comcast.net'>steve.barber@comcast.net</a>. ";

                MailMessage m = new MailMessage();
                m.From = new MailAddress("registration@learntoridetn.com");
                m.To.Add(new MailAddress(regList.FirstOrDefault().EMAIL));
                m.Bcc.Add(new MailAddress(owneremail));
                m.Bcc.Add(new MailAddress(adminmail));
                m.Subject = "vMTS Registration: #" + id;
                m.IsBodyHtml = true;
                m.Body = body;

                SmtpClient sc = new SmtpClient(smtp);
                sc.Credentials = new System.Net.NetworkCredential(postmaster, pass);
                sc.Send(m);

                msg = "Success";

                // SEND MESSAGE TO vMTS ADMINISTRATORS
                SendAdminMessage(id);

            }
            catch (Exception e)
            {
                msg = e.Message;
            }
        }

        public string PaymentConfirmation(Int64 id)
        {
            var regList = new List<Registration>();
            var payList = new List<vMTS.Models.RegistrationModel.PaymentConfirm>();

            regList = r.GetConfirmation(id);
            payList = r.GetRegistrationPayment(id);
            bool send = true;

            var msg = "";
            var body = "";//<h1>THIS IS ONLY A TEST</h1>
            try
            {
                body += "<h1>Payment Confirmation</h1>";
                if (payList.FirstOrDefault().PMT_TYPE == "Gift")
                {
                    body += "Do Not Send";
                    send = false;

                }
                else if (payList.FirstOrDefault().PMT_TYPE == "Pre-Paid")
                {
                    body += "Do Not Send";
                    send = false;

                }
                else if (payList.FirstOrDefault().PMT_TYPE == "Check")
                {
                    body += "<table><tr><td>" + string.Format("{0:C}", payList.FirstOrDefault().TOTAL) + " has been collected</td></tr>";
                    body += "<tr><td>Processed Date: " + string.Format("{0:D}", DateTime.Now + "</td></tr>");
                    body += "<tr><td>" + payList.FirstOrDefault().PMT_TYPE + "</td></tr>";
                    body += "</table>";

                    send = true;

                }
                else if(payList.FirstOrDefault().PMT_TYPE == "Visa" || payList.FirstOrDefault().PMT_TYPE == "MasterCard" || payList.FirstOrDefault().PMT_TYPE == "Discover") // CREDIT CARD PAYMENT 
                {
                    body += "<table><tr><td>" + string.Format("{0:C}", payList.FirstOrDefault().TOTAL) + " has been charged to the following credit/debit card.</td></tr>";
                    body += "<tr><td>Processed Date: " + string.Format("{0:D}", DateTime.Now + "</td></tr>");
                    body += "<tr><td>" + payList.FirstOrDefault().PMT_TYPE + "</td></tr>";
                    body += "<tr><td>" + payList.FirstOrDefault().CARDNAME + "</td></tr>";
                    body += "<tr><td>" + payList.FirstOrDefault().CARDNUMBER + "</td></tr>";
                    body += "<tr><td>The transaction will appear on your statement as S. Barber dba VMTS. If this card is paying for more than one registration, your statement will be a combined amount.</td></tr>";
                    body += "</td></tr></table>";

                    send = true;

                }
                else
                {
                    body += "<table><tr><td>" + string.Format("{0:C}", payList.FirstOrDefault().TOTAL) + " has been collected</td></tr>";
                    body += "<tr><td>Processed Date: " + string.Format("{0:D}", DateTime.Now + "</td></tr>");
                    body += "<tr><td>" + payList.FirstOrDefault().PMT_TYPE + "</td></tr>";
                    body += "</table>";

                    send = true;

                }

                body += "<table><th>Important Refund/Cancellation Policy</th>";
                body += "<tr><td>NO REFUNDS will be made, except for course cancellations, at least 14 days in advance of the scheduled class date. One reschedule will be permitted if notified at least 24 hours in advance of the class start time. The cost of the reschedule is $100. If a request is not made within the specified time period, the class fee will be forfeited and a new fee will be charged for a later class.</td></tr>";
                body += "<tr><td>Students who are unable to meet the minimum physical requirements in the opinion of the RiderCoach, or students whose behavior pose a hazard to themselves and/or other students will be asked to discontinue the riding portion of the class with NO REFUND GIVEN. They may stay to observe the remainder of the class but will not be certified for completion. Students must successfully complete the entire class (including the written and riding skills evaluation) to receive a completion certificate and MSF card completion card.</tr></td></table>";

                body += "<table><tr><td>Do not reply to this email.  If you need assistance, please contact us here: <a href='www.learntoridetn.com/home/contact' target='_blank'>learntoridetn.com/home/contact</a></td></tr></table>";
                
                MailMessage m = new MailMessage();
                m.From = new MailAddress("registration@learntoridetn.com");
                m.To.Add(new MailAddress(regList.FirstOrDefault().EMAIL));
                m.Bcc.Add(new MailAddress(adminmail));
                m.Bcc.Add(new MailAddress(owneremail));
                m.Subject = "Payment Confirmation for Registration: #" + id;
                m.IsBodyHtml = true;
                m.Body = body;

                SmtpClient sc = new SmtpClient(smtp);
                sc.Credentials = new System.Net.NetworkCredential(postmaster, pass);

                if (send == true)
                {
                    sc.Send(m);
                }
                
                msg = "Sent";

            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return msg;
        }
        //Administration notification for registrations
        public void SendAdminMessage(Int64 id)
        {
            var regList = new List<Registration>();

            regList = r.GetConfirmation(id);

            var msg = "";
            var body = "";//<H1>THIS IS ONLY A TEST</H1>
            try
            {
                body += "<h1>ALERT ** New Class Registration **</h1>";
                body += "<table><tr><td>Registration #:</td><td>" + id + "</td></tr>";
                body += "<tr><td>Registration Date:</td><td>" + regList.FirstOrDefault().REG_DATE + "</td></tr></table>";
                body += "<table><tr><td>Course Type:</td><td>" + regList.FirstOrDefault().CLASS_TYPE + "</td></tr>";
                body += "<tr><td>Class Date:</td><td>" + regList.FirstOrDefault().CLASS_DAY + "</td></tr>";
                body += "<tr><td>Participant Name:</td><td>" + regList.FirstOrDefault().NAME + "</td></tr>";
                body += "<tr><td>Participant Phone:</td><td>" + regList.FirstOrDefault().PHONE + "</td></tr>";

                body += "<tr><td>Participant Age:</td><td>" + regList.FirstOrDefault().AGE + "</td></tr></table>";

                MailMessage m = new MailMessage();
                m.From = new MailAddress("registration@learntoridetn.com");
                m.To.Add(new MailAddress(adminmail));
                m.Bcc.Add(new MailAddress(owneremail));
                m.Subject = "New Class Registration: #" + id;
                m.IsBodyHtml = true;
                m.Body = body;

                SmtpClient sc = new SmtpClient(smtp);
                sc.Credentials = new System.Net.NetworkCredential(postmaster, pass);
                sc.Send(m);

                msg = "Success";

            }
            catch (Exception e)
            {
                msg = e.Message;
            }
        }

        public void SendAdminPayment(Int64 id)
        {
        }
        public string SendConfirmationCode(string email, string code)
        {
            var msg = "";
            try { 
            
            MailMessage m = new MailMessage();
            m.From = new MailAddress("support@learntoridetn.com");
            m.To.Add(new MailAddress(email));
            m.Subject = "vMTS Support";
            m.IsBodyHtml = true;
            m.Body = code;

            SmtpClient sc = new SmtpClient(smtp);
            sc.Credentials = new System.Net.NetworkCredential(postmaster, pass);
            sc.Send(m);

            msg = "Code Sent";

            }
            catch (Exception e)
            {
                msg = "Code Could Not Be Sent: " + e.Message;
            }

            return msg;
        }

        public void SendErrorEmail(string errorMsg,string method)
        {
            try
            {
                var body = "<h1>A user encountered an issue</h1>";//
                body += "<h3>A more detailed message has been logged in the database.</h3>";
                body += "<h3>Web Function: " + method + "</h3>";
                body += "<h3>Error Message Generated: " + errorMsg + "</h3>";
                body += "<h1>DO NOT REPLY TO THIS EMAIL</h1>";
                body += "<h3>This message is for your information, it's likely action is required to correct.</h3>";

                MailMessage m = new MailMessage();
                m.From = new MailAddress("noreply@learntoridetn.com");
                //m.Bcc.Add(new MailAddress(adminmail));
                //m.Bcc.Add(new MailAddress(owneremail));
                m.Bcc.Add(new MailAddress(devmail));

                m.Subject = "vMTS Web Site Error";
                m.IsBodyHtml = true;
                m.Body = body;

                SmtpClient sc = new SmtpClient(smtp);
                sc.Credentials = new System.Net.NetworkCredential(postmaster, pass);

                    sc.Send(m);

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

        }

        public void LogErrorToDB(string logMethod, string logMessage, string logStackTrace)
        {
            using (vmts_dataDataContext db = new vmts_dataDataContext())
            {
                errorLogging logTBL = new errorLogging
                {
                    logMethod = logMethod,
                    logMessage = logMessage,
                    logStackTrace = logStackTrace,
                    logDate = DateTime.Now
                };

                db.errorLoggings.InsertOnSubmit(logTBL);
                db.SubmitChanges();
            }
        }
    }
}