using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace DojoActivityCenter.Models
{
    public class User{
    
        [Key]
        public int UserId {get;set;}
        
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "No numbers allowed in Name Field")]
        [MinLength(2, ErrorMessage="Name must be at least 2 characters long!")]
        [Display(Name="Name")]
        public string Name {get;set;}

        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string Email {get;set;}

        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at leat 8 characters and contain 3 or 4 of the following: uppercase(A-Z) or lower case(a-z) letters, numbers(0-9) or special characters (e.g. !@#&%*)")]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [NotMapped]
        [Required]
        [Compare("Password", ErrorMessage="Passwords must match!")]
        [DataType(DataType.Password)]
        [Display(Name="PW Confirmation")]
        public string Confirm {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Roster> Events {get;set;}
        public List<Activity> CreatedActivities {get;set;}
    }
}
