using System;
using System.ComponentModel.DataAnnotations;

namespace DojoActivityCenter.Models {

    public class LoginUser {
        
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string Login_Email {get;set;}

        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at leat 8 characters and contain 3 or 4 of the following: uppercase(A-Z) or lower case(a-z) letters, numbers(0-9) or special characters (e.g. !@#&%*)")]
        [DataType(DataType.Password)]
        public string Login_Password {get;set;}
    }
}
