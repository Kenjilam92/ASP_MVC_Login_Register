using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class LoginUser
    {
        [Required][DataType(DataType.EmailAddress)]
        [Display(Name="Email")]
        public string Email {get;set;}
        [Required][MinLength(8,ErrorMessage="Password must has at least 8 characters")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string Password {get; set;}
    }
}