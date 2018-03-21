using System;
using System.ComponentModel.DataAnnotations;

namespace weddingPlanner.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        public string FirstName {get;set;}

        [Required]
        [MinLength(2)]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [Compare("Password", ErrorMessage = "Password must match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Comfirm Password")]
        public string Confirm {get;set;}
    }
    
}