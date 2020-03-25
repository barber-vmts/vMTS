using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vMTS.Models
{

    public class VerifyCodeViewModel
    {

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        public string CodeSent { get; set; }

    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public bool IsValidated { get; set; }
    }

    public class LoginPartialViewModel
    {
        public string User { get; set; }
        public string RoleName { get; set; }


    }
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }

    public class NotRegisteredViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string ReturnUrl { get; set; }


    }

}
