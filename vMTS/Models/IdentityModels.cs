using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using vMTS.Controllers;

namespace vMTS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser
    {
        public bool ReadTempContract(string code)
        {
            string userSettings = "None";
            bool match = false;
            if (HttpContext.Current.Request.Cookies["vmts_xref_temp"] != null)
            {
                if (HttpContext.Current.Request.Cookies["vmts_xref_temp"]["grant"] != null)
                { userSettings = HttpContext.Current.Request.Cookies["vmts_xref_temp"]["grant"]; }

                if (userSettings == code)
                {
                    match = true;
                }
                else
                {
                    match = false;
                }
            }
            return match;
        }

        public string ReadContract()
        {
                 string userSettings="None";
            if (HttpContext.Current.Request.Cookies["vmts_xref"] != null)
            {
                if (HttpContext.Current.Request.Cookies["vmts_xref"]["grant"] != null)
                { userSettings = HttpContext.Current.Request.Cookies["vmts_xref"]["grant"]; }
            }
            return userSettings;
        }

        public void SetTempContract(string code, string em)
        {
            HttpCookie c = new HttpCookie("vmts_xref_temp");
            c["grant"] = code;
            //c["e"] = em;
            c.Expires = DateTime.Now.AddMinutes(15);

            HttpContext.Current.Response.Cookies.Add(c);

        }
        public void SetContract()
        {
            string code = Guid.NewGuid().ToString();

            HttpCookie c = new HttpCookie("vmts_xref");
            c["grant"] = code;
            c.Expires = DateTime.Now.AddMonths(6);

            HttpContext.Current.Response.Cookies.Add(c);

        }

        public void RemoveTempContract()
        {
            if (HttpContext.Current.Request.Cookies["vmts_xref_temp"] != null)
            {
                HttpContext.Current.Request.Cookies["vmts_xref_temp"].Expires.AddDays(-1d);
            }
        }

        public bool ReadAdminTempContract(string code)
        {
            string userSettings = "None";
            bool match = false;
            if (HttpContext.Current.Request.Cookies["vmts_xref_temp_admin"] != null)
            {
                if (HttpContext.Current.Request.Cookies["vmts_xref_temp_admin"]["grant"] != null)
                { userSettings = HttpContext.Current.Request.Cookies["vmts_xref_temp_admin"]["grant"]; }

                if (userSettings == code)
                {
                    match = true;
                }
                else
                {
                    match = false;
                }
            }
            return match;
        }

        public void SetAdminTempContract(string code)
        {
            HttpCookie c = new HttpCookie("vmts_xref_temp_admin");
            c["grant"] = code;
            c.Expires = DateTime.Now.AddMinutes(5);

            HttpContext.Current.Response.Cookies.Add(c);

        }
        public void RemoveAdminTempContract()
        {
            if (HttpContext.Current.Request.Cookies["vmts_xref_temp_admin"] != null)
            {
                HttpContext.Current.Request.Cookies["vmts_xref_temp_admin"].Expires.AddDays(-1d);
            }
        }

    }

    public class SignInManager
    {
        public static string CurrentUser { get; set; }
        public static string CurrentRoleName { get; set; }

        public static bool CurrentValid { get; set; }

        public static async Task<string> ValidateUser(string email)
        {
            var u = "Fail";
            using(ID_dataDataContext db = new ID_dataDataContext()){
                var sql = (from c in db.ValidateUser_vws
                           where c.UserName == email
                           select (c)).ToList();
                // USER IS REGISTERED
                if (sql.Count >= 1) { 
                    ApplicationUser usr = new ApplicationUser();
                    u = usr.ReadContract();
                    await Task.Delay(500);

                    if (u == "None")
                    {
                        u = "Code";
                    }
                    else if (sql.Count == 1) // USER IS REGISTERED WITH THE EMAIL GIVEN
                    {
                        LoginPartialViewModel lp = new LoginPartialViewModel();
                        lp.User = sql[0].UserName;
                        lp.RoleName = sql[0].RoleName;

                        CurrentUser = lp.User;
                        CurrentRoleName = lp.RoleName;
                        
                        HttpContext.Current.Session["CurrentRoleName"] = lp.RoleName;  
                        
                        LoginViewModel l = new LoginViewModel();
                        l.IsValidated = true;
                        CurrentValid = l.IsValidated;

                        u = "Pass";

                    }
                    else
                    {
                        u = "Fail";
                    }
                }
                // USER IS NOT REGISTERED
                else
                {
                    u = "Not Registered"; 
                }

            }
            
            return u;
        }
        public static async Task<string> PasswordSignInAsync(string email)
        {
            string r;
            r = await ValidateUser(email);
            return r;
        }

        public static void CheckValidCode(string code)
        {
                ApplicationUser usr = new ApplicationUser();
                bool r = usr.ReadTempContract(code);

                if (r) { 
                    // CODE HAS BEEN VERIFIED, SO SET THE 6 MONTH CONTRACT
                    usr.SetContract();

                    //RedirectResult("~/account/login");
                }

        }
        
        // VERIFY THE CODE ENTERED BY THE USER
        public static async Task<string> TwoFactorSignInAsync(string code)
        {
            string r;
            try {
                CheckValidCode(code);
                await Task.Delay(1000);
                r = "Success";
            }
            catch
            {
                r = "Fail";
            }

            return r;
        }
    }

    public class ApplicationDbContext : ID_dataDataContext
    {
        public ApplicationDbContext()
            : base("ID_dataDataContext")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}