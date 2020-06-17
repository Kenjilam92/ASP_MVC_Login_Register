
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Login.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required][MinLength(2,ErrorMessage="First Name must has at least 2 characters")]
        [Display(Name="First Name")]
        public string FirstName {get;set;}
        [Required][MinLength(2,ErrorMessage="Last Name must has at least 2 characters")]
        [Display(Name="Last Name")]
        public string LastName {get;set;}
        [Required][DataType(DataType.EmailAddress)]
        [Display(Name="Email")]
        public string Email {get;set;}
        [Required][MinLength(8,ErrorMessage="Password must has at least 8 characters")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string Password {get; set;}
        public DateTime CreateAt {get;set;} = DateTime.Now;
        public DateTime UpdateAt {get;set;} = DateTime.Now;
        // Not store in Database
        [NotMapped][Compare("Password")][DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string CfPassword {get; set;}
    }
}