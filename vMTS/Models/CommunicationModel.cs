using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;

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
        string pass = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(WebConfigurationManager.AppSettings["SendEmailPass"]));

        public void SendRegistrationMessage(Int64 id)
        {
            try
            {
                var body = "";//<h1>THIS IS ONLY A TEST</h1>
                var websiteURL = WebConfigurationManager.AppSettings["WebsiteURL"];
                var regList = r.GetConfirmation(id);
                Registration registration = regList.FirstOrDefault();
                
                string confirmUrl = websiteURL + "Registration/RegistrationEmailConfirmation/" + registration.REGISTRATION_EMAIL_ID;

                body = "<table cellspacing='0' cellpadding='0' width='100%'><tr><td style='text-align:center;'><center><a href='https://www.learntoridetn.com'><img height='60' src='https://www.learntoridetn.com/images/logo.jpg' alt='Volunteer Motorcycle Training Services'></a></center></td></tr></table>";
                
                body += "<table cellspacing='0' cellpadding='0' width='100%' style='background-color: #f7f7f7;padding: 20px 30px 30px;font-size:14px;'>";
                body += "<tr><td>";
                    body += "Dear " + registration.NAME + ",";
                    body += "<p>Thank you for regisitering for the <b>" + registration.CLASS_TYPE + "</b> class. We are looking forward to seeing you in your class. Below is the information for your class registration.</p>";
                body += "</td></tr>";
                
                body += "<tr><td>";
                    body += "<table cellpadding='10'><tr><td><b>Registration Number:</b></td><td>" + id + "</td></tr>";
                    body += "<tr><td><b>Registration Date:</b></td><td>" + registration.REG_DATE + "</td></tr>";
                    body += "<tr><td><b>Course Type:</b></td><td>" + registration.CLASS_TYPE + "</td></tr>";
                    body += "<tr><td><b>Class Date:</b></td><td>" + registration.CLASS_DAY + "</td></tr>";
                    body += "<tr><td><b>Participant Name:</b></td><td>" + registration.NAME + "</td></tr>";
                    body += "<tr><td><b>Participant Phone:</b></td><td>" + registration.PHONE + "</td></tr></table>";
                body += "</td></tr>";
                
                body += "<tr><td>";
                    body += "<p>The next step to make this official is for you to confirm your registration. In the confirmation please read all the text as it contains important instructions.</p>";
                    body += "<p><a href=" + confirmUrl + ">CONFIRM HERE</a><p>";
                    body += "<p>Or</p>";
                    body += "<span> you can copy paste below URL to your browser. Please do not share this link to anyone. It is unique to you. </span> <br />";
                    body += "<a href=' + confirmUrl + '>"+ confirmUrl +"</a>";
                    //body += "<b><span style=\"margin-top:10px;margin-bottom:10px;\">Notice: This is a confirmation of your registration. Please <a href=" + confirmUrl + "> click here </a> or copy paste thid ("+confirmUrl+") URL to your browser to confirm receipt and to open further instructions. Read all the text as it contains important instructions.";
                    body += "<p>Payment has not been processed. You will receive a separate email message when payment has been processed.</p>";
                body += "</td></tr>";
                body += "<tr style='line-height: 8px;'><td>";
                    body += "<p>Regards,";
                    body += "<p>Volunteer Motorcycle Training Services Team</p>";
                    body += "<a href='tel:+16154149042'>615.414.9042</a> <br /> <br />";
                    body += "<a href='https://www.facebook.com/Volunteer-Motorcycle-Training-Services-372371187072/timeline' target='_blank'><img src='https://www.learntoridetn.com/Content/images/Facebook-icon.png' alt='Facebook'></a><a href='https://twitter.com/barbervmts' target='_blank'><img src='https://www.learntoridetn.com/Content/images/Twitter-icon.png' alt='Twitter'></a></div>";
                body += "</td></tr>";
                body += "</table>";

                MailMessage m = new MailMessage();
                m.From = new MailAddress("registration@learntoridetn.com");
                m.To.Add(new MailAddress(registration.EMAIL));
                m.Bcc.Add(new MailAddress(owneremail));
                m.Bcc.Add(new MailAddress(adminmail));
                m.Subject = "Volunteer Motorcycle Training Services Registration";
                m.IsBodyHtml = true;
                m.Body = body;
                
                SmtpClient sc = new SmtpClient(smtp);
                sc.Credentials = new System.Net.NetworkCredential(postmaster, pass);
                sc.Send(m);

                // SEND MESSAGE TO vMTS ADMINISTRATORS
                SendAdminMessage(id);
            }
            catch (Exception error)
            {
                LogErrorToDB("SendRegistrationMessage()", error.Message, error.StackTrace);
            }
        }

        public string SendClassDetials(Guid id)
        {
            try
            {
                var body = "";//<h1>THIS IS ONLY A TEST</h1>
                var regList = r.GetConfirmation(id);

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

                body += "<table><th style='text-align: center;'>Class Details</th>";
                body += "<tr><td>Class will begin at " + string.Format("{0:t}", regList.FirstOrDefault().CLASS_START_TIME) + " on " + string.Format("{0:D}", regList.FirstOrDefault().CLASS_START_DATE) + ".  Plan to arrive a little early to complete some paper work before class begins.</td></tr>";
                body += "<tr><td>The address of the training site is 270 E. Main Street, Hendersonville, TN 37075.  Note that GPS will take you to the main entrance, west parking lot; we are located on the other side of the plant in the east parking lot.  Come to the Shipping/Receiving entrance next to Pinnacle Bank, across from Simmons Bank.</td></tr>";

                if (regList.FirstOrDefault().CLASS_TYPE == "Basic RiderCourse II")
                {
                    body += "<tr><td>How to come prepared:<ul><li>DOT street legal motorcycle.</li><li>DOT helmet - you must provide your own helmet<br>";
                }
                else if (regList.FirstOrDefault().CLASS_TYPE == "3 Wheel (Trike) Course")
                {
                    body += "<tr><td>Riding gear includes:<ul><li>DOT street legal 3-wheel motorcycle.</li><li>DOT helmet - you must provide your own helmet<br>";
                }
                else
                {
                    body += "<tr><td>Riding gear includes:<ul><li>DOT helmet - you must provide your own helmet<br>";
                }

                body += "<b>Note:</b> If you do not have a helmet, show this confirmation to the staff at Cycle Gear to receive a 10% discount toward a new helmet.  Cycle Gear is located at Rivergate Station, 1677 Gallatin Pike N. Madison, TN 37115</li>";
                body += "<li>Shatter proof eyewear if using open face helmet</li><li>Long sleeve shirt or jacket must be worn while sitting on the motorcycle and riding.</li><li>Full fingered gloves; cut off gloves not allowed</li><li>Long pants- no holes or exposed skin</li><li>Over the ankle boots with good grip soles</li></ul></td></tr>";

                if (regList.FirstOrDefault().CLASS_TYPE == "3 Wheel (Trike) Course")
                {
                    body += "<tr><td><b>You Will not be allowed to participate in the riding exercises and will forfeit your fee without a street legal 3-wheel motorcycle and all the gear.</b></td></tr>";
                }
                else
                {
                    body += "<tr><td><b>You Will not be allowed to participate in the riding exercises and will forfeit your fee without all the gear.</b></td></tr>";
                }



                body += "<tr><td>Please review the Motorcycle Safety Foundation Liability Waiver and Indemnification form by clicking on the link <a href='https://www.learntoridetn.com/images/Liability%20Waiver/MSF-RiderCourseWaiver.pdf' target='_blank'>Liability Waiver</a>. You will sign the form upon your arrival to class.</td></tr>";

                if (regList.FirstOrDefault().CLASS_TYPE == "Basic RiderCourse I")
                {
                    body += "<tr><td>A requirement to successfully complete the Basic Rider Course and receive a Tennessee Department of Safety completion certificate good toward licensing is to complete the online eCourse located in an accompanying email. We will also include a crossword puzzle in this email to help you prepare for the test.</td></tr>";
                    body += "<tr><td>Requires a minimum of 4 riders for class to be held.</td></tr>";

                    body += "<tr><td>To successfully complete the Basic RiderCourse, you must complete the MSF eCourse (online) and complete the Tennessee Department of Safety and Homeland Security motorcycle knowledge test.</td></tr>";
                    body += "<tr><td><ul><li>Click on <a href='https://www.learntoridetn.com/images/MSF%20eCourse%20Instructions/MSF%20eCourse%20Instructions.pdf' target='_blank'>MSF eCourse</a> for instructions for completing the online class.</li>";
                    body += "<li>Click <a href='https://www.learntoridetn.com/images/TN%20Knowledge%20Test%20Prep/TN%20Knowledge%20Test%20Prep.pdf' target='_blank'>Knowledge Test</a> as preparation for the Tennessee knowledge test.</li></ul></td></tr>";


                    body += "<tr><td>Learning to ride a motorcycle is challenging both physically and mentally, but certainly attainable. We ask that you not schedule any activities during the two days of class, including work, parties and late night outings. Get a good nights rest the night before the class and the night following the first day. Successful completion is up to you.</td><tr>";
                }

                if (regList.FirstOrDefault().CLASS_TYPE == "Basic RiderCourse II" || regList.FirstOrDefault().CLASS_TYPE == "Advanced RiderCourse")
                {
                    if (regList.FirstOrDefault().EVAL == "N")
                    {
                        body += "<tr><td>A requirement to successfully complete the Basic RiderCourse II and receive a Motorcycle Safety Foundation (MSF) completion card good toward an insurance discount and/or employment requirement is on-time attendance and participation in all riding exercises and discussions.</td></tr>";
                    }
                    //body += "<tr><td>You must come with a DOT street legal motorcycle with all components in good working order, i.e. tires with good tread, both brakes working, all lights working, and plenty of gas in the tank, no physical damage, etc.</td></tr>";
                    //body += "<tr><td>You must provide registration and proof of insurance. If you have a borrowed or rented bike, you must bring consent from the owner or a copy of the rental agreement.</td></tr>";
                    body += "<tr><td>Requires a minimum of 4 riders for class to be held.</td></tr>";

                    if (regList.FirstOrDefault().EVAL == "Y")
                    {
                        body += "<tr><td>To successfully complete the Basic RiderCourse, you must complete the MSF eCourse (online) and complete the Tennessee Department of Safety and Homeland Security motorcycle knowledge test.</td></tr>";
                        body += "<tr><td><ul><li>Click on <a href='https://www.learntoridetn.com/images/MSF%20Street%20Strategies%20eCourse/Enroll_Access_Street_Strategies_eCourse_02.pdf' target='_blank'>Street Strategies eCourse</a> for instructions for completing the online class.</li>";
                        body += "<li>Click <a href='https://www.learntoridetn.com/images/TN%20Knowledge%20Test%20Prep/TN%20Knowledge%20Test%20Prep.pdf' target='_blank'>Knowledge Test</a> as preparation for the Tennessee knowledge test.</li></ul></td></tr>";
                        //body += "<tr><td>Learning to ride a motorcycle is challenging both physically and mentally, but certainly attainable. We ask that you not schedule any activities during the two days of class, including work, parties and late night outings. Get a good nights rest the night before the class and the night following the first day. Successful completion is up to you.</td><tr>";
                    }
                }
                if (regList.FirstOrDefault().CLASS_TYPE == "3 Wheel (Trike) Course")
                {
                    body += "<tr><td>A requirement to successfully complete the Basic Rider Course and receive a Tennessee Department of Safety completion certificate good toward licensing is to complete the online eCourse located in an accompanying email. We will also include a crossword puzzle in this email to help you prepare for the test.</td></tr>";
                    body += "<tr><td>Requires a minimum of 4 riders for class to be held.</td></tr>";


                    body += "<tr><td>To successfully complete the Basic RiderCourse, you must complete the MSF eCourse (online) and complete the Tennessee Department of Safety and Homeland Security motorcycle knowledge test.</td></tr>";
                    body += "<tr><td><ul><li>Click on <a href='https://www.learntoridetn.com/images/MSF%203Wheel%20eCourse/MSF%203Wheel%20Instructions.pdf' target='_blank'>3-Wheel eCourse</a> for instructions for completing the online class.</li>";
                    body += "<li>Click <a href='https://www.learntoridetn.com/images/TN%20Knowledge%20Test%20Prep/TN%20Knowledge%20Test%20Prep.pdf' target='_blank'>Knowledge Test</a> as preparation for the Tennessee knowledge test.</li></ul></td></tr>";
                    body += "<tr><td>Learning to ride a motorcycle is challenging both physically and mentally, but certainly attainable. We ask that you not schedule any activities during the two days of class, including work, parties and late night outings. Get a good nights rest the night before the class and the night following the first day. Successful completion is up to you.</td><tr>";

                    //body += "<tr><td>You must come with a DOT street legal trike with all components in good working order, i.e. tires with good tread, both brakes working, clutch properly working, and plenty of gas in the tank, no physical damage, etc.</td></tr>";
                    //body += "<tr><td>You must provide registration and proof of insurance. If you have a borrowed or rented trike, you must bring consent from the owner or a copy of the rental agreement.</td></tr>";

                    //body += "<tr><td>To help better prepare for class, we suggest reviewing the online study guide at <a href='http://www.msf-usa.org/downloads/3WBRC_Student_Handbook_2010.pdf' target='_blank'>www.msf-usa.org/downloads/3WBRC_Student_Handbook_2010.pdf</a> with study questions beginning on page 51.</td></tr>";
                    //body += "<tr><td>Another good source is the MSF 48 question review at <a href='http://www.msf-usa.org/BRCQuiz.aspx#/H3w0CSdF1x/PlnH05KLo1/SfsW80tFCk' target='_blank'>www.msf-usa.org/BRCQuiz.aspx#/H3w0CSdF1x/PlnH05KLo1/SfsW80tFCk</a>.</td></tr>";
                    //body += "<tr><td>Learning to ride a 3 wheel motorcycle is challenging both physically and mentally, but certainly attainable. We ask that you not schedule any activities during the two days of class, including work, parties and late night outings. Get a good nights rest the night before the class and the night following the first day. Successful completion is up to you.</td><tr>";
                }

                if (regList.FirstOrDefault().CLASS_TYPE == "Skills Practice and Improvement")
                {
                    body += "<tr><td>Requires a minimum of 10 riders for class to be held.</td></tr>";
                }
                body += "<tr><td>We will take lots of breaks. Feel free to bring snacks. We will provide water.</td></tr>";

                body += "</table>";

                body += "<table><tr><td><h3>Important Refund/Cancellation Policy</h3></td></tr>";
                body += "<tr><td>NO REFUNDS will be made, except for course cancellations, at least 7 days in advance of the scheduled class date. One re-schedule will be permitted if notified at least 24 hours in advance of the class start time. The cost of the reschedule is $100. If a request is not made within the specified time period, the class fee will be forfeited and a new fee will be charged for a later class.</td></tr>";
                body += "<tr><td>Students who are unable to meet the minimum physical requirements in the opinion of the RiderCoach, or students whose behavior pose a hazard to themselves and/or other students will be asked to discontinue the riding portion of the class with NO REFUND GIVEN. They may stay to observe the remainder of the class but will not be certified for completion. Students must successfully complete the entire class (including the MSF eCourse, written and riding skills evaluation) to receive a completion certificate and MSF completion card.</tr></td></table>";

                body += "<table>";
                body += "<tr><td>If links in the email do not work, copy and paste them into your browser do not reply to this email.  If you need assistance, please notify Steve Barber, site administrator, by phone or text to 615.414.9042 or email at <a href='mailto:steve.barber@comcast.net'>steve.barber@comcast.net</a>.</tr></td>";
                body += "</table>";

                return body;
            }
            catch (Exception error)
            {
                LogErrorToDB("SendClassDetials()", error.Message, error.StackTrace);
                return "";
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
                else if (payList.FirstOrDefault().PMT_TYPE == "Visa" || payList.FirstOrDefault().PMT_TYPE == "MasterCard" || payList.FirstOrDefault().PMT_TYPE == "Discover") // CREDIT CARD PAYMENT 
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

                if ((regList.FirstOrDefault().EVAL == "Y" && regList.FirstOrDefault().CLASS_TYPE == "Basic RiderCourse II") ||
                    (regList.FirstOrDefault().EVAL == "Y" && regList.FirstOrDefault().CLASS_TYPE == "Basic RiderCourse I") ||
                    (regList.FirstOrDefault().EVAL == "Y" && regList.FirstOrDefault().CLASS_TYPE == "3 Wheel (Trike) Course"))
                {
                    body += "<table><tr><td>Students must successfully complete the entire class (including the MSF eCourse, written and riding skills evaluation) to receive a completion certificate and MSF completion card.</td></tr>";
                    body += "<br />";
                    body += "<tr><td>If links in the email do not work, copy and paste them into your browser do not reply to this email.  If you need assistance, please notify Steve Barber, site administrator, by phone or text to 615.414.9042 or email at <a href='mailto:steve.barber@comcast.net'>steve.barber@comcast.net</a>.</tr></td>";
                    body += "</table>";

                    body += "<table><th>Important Refund/Cancellation Policy</th>";
                    body += "<tr><td>NO REFUNDS will be made, except for course cancellations, at least 7 days in advance of the scheduled class date. One re-schedule will be permitted if notified at least 24 hours in advance of the class start time. The cost of the reschedule is $100. If a request is not made within the specified time period, the class fee will be forfeited and a new fee will be charged for a later class.</td></tr>";
                    body += "<br />";
                    body += "<tr><td>Students who are unable to meet the minimum physical requirements in the opinion of the RiderCoach, or students whose behavior pose a hazard to themselves and/or other students will be asked to discontinue the riding portion of the class with NO REFUND GIVEN. They may stay to observe the remainder of the class but will not be certified for completion.</tr></td></table>";

                    send = true;
                }
                else if (regList.FirstOrDefault().EVAL == "N" && regList.FirstOrDefault().CLASS_TYPE == "Basic RiderCourse II")
                {
                    body += "<table><th>Important Refund/Cancellation Policy</th>";
                    body += "<tr><td>NO REFUNDS will be made, except for course cancellations, at least 7 days in advance of the scheduled class date. One re-schedule will be permitted if notified at least 24 hours in advance of the class start time. The cost of the reschedule is $100. If a request is not made within the specified time period, the class fee will be forfeited and a new fee will be charged for a later class.</td></tr>";
                    body += "<br />";
                    body += "<tr><td>Students who are unable to meet the minimum physical requirements in the opinion of the RiderCoach, or students whose behavior pose a hazard to themselves and/or other students will be asked to discontinue the riding portion of the class with NO REFUND GIVEN. They may stay to observe the remainder of the class but will not be certified for completion. Students must successfully complete the entire class to receive a MSF completion card.</tr></td></table>";
                    body += "<table><tr><td>If links in the email do not work, copy and paste them into your browser do not reply to this email.  If you need assistance, please notify Steve Barber, site administrator, by phone or text to 615.414.9042 or email at <a href='mailto:steve.barber@comcast.net'>steve.barber@comcast.net</td></tr></table>";
                }
                else
                {
                    body += "<table><th>Important Refund/Cancellation Policy</th>";
                    body += "<tr><td>NO REFUNDS will be made, except for course cancellations, at least 14 days in advance of the scheduled class date. One reschedule will be permitted if notified at least 24 hours in advance of the class start time. The cost of the reschedule is $100. If a request is not made within the specified time period, the class fee will be forfeited and a new fee will be charged for a later class.</td></tr>";
                    body += "<br />";
                    body += "<tr><td>Students who are unable to meet the minimum physical requirements in the opinion of the RiderCoach, or students whose behavior pose a hazard to themselves and/or other students will be asked to discontinue the riding portion of the class with NO REFUND GIVEN. They may stay to observe the remainder of the class but will not be certified for completion. Students must successfully complete the entire class (including the written and riding skills evaluation) to receive a completion certificate and MSF card completion card.</tr></td></table>";

                    body += "<table><tr><td>If links in the email do not work, copy and paste them into your browser do not reply to this email.  If you need assistance, please notify Steve Barber, site administrator, by phone or text to 615.414.9042 or email at <a href='mailto:steve.barber@comcast.net'>steve.barber@comcast.net</td></tr></table>";
                }

                MailMessage m = new MailMessage();
                m.From = new MailAddress("registration@learntoridetn.com");
                m.To.Add(new MailAddress(regList.FirstOrDefault().EMAIL));
                m.Bcc.Add(new MailAddress(adminmail));
                m.Bcc.Add(new MailAddress(owneremail));
                m.Subject = "Volunteer Motorcycle Training Services Payment Confirmation";
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
            catch (Exception error)
            {
                msg = error.Message;
                LogErrorToDB("PaymentConfirmation()", error.Message, error.StackTrace);
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
            catch (Exception error)
            {
                LogErrorToDB("SendAdminMessage()", error.Message, error.StackTrace);
            }
        }

        public void SendAdminPayment(Int64 id)
        {
        }
        public string SendConfirmationCode(string email, string code)
        {
            var msg = "";
            try
            {

                MailMessage m = new MailMessage();
                m.From = new MailAddress("support@learntoridetn.com");
                m.To.Add(new MailAddress(email));
                m.Subject = "vMTS Support";
                m.IsBodyHtml = true;
                m.Body = code;

                SmtpClient sc = new SmtpClient(smtp);
                sc.Credentials = new System.Net.NetworkCredential(postmaster, pass);
                //sc.Send(m);

                msg = "Code Sent";

            }
            catch (Exception error)
            {
                LogErrorToDB("SendConfirmationCode()", error.Message, error.StackTrace);
                msg = "Code Could Not Be Sent: " + error.Message;
            }

            return msg;
        }

        public void SendErrorEmail(string errorMsg, string method)
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