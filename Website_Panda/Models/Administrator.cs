using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class Administrator
    {
        [Key]
        public long IDUser { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Please enter UserName!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter Email!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Password must contain atleast one upper character and number!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm Password!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Password must contain atleast one upper character and number!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        public DateTime? CreatedDate { get; set; }
               
        [StringLength(20)]
        public string GroupID { get; set; }
        public double? Score { get; set; }
        public bool? Status { get; set; }
    }
}