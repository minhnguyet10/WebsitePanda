using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class Customer
    {
        [Key]
        public long IdCus { get; set; }

        [StringLength(50)]
        //[Required(ErrorMessage = "UserName is not null! Please enter UserName!")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Password is not null! Please enter Password!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Password must contain atleast one upper character and number!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Please confirm Password!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Password must contain atleast one upper character and number!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
             
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage = "Invalid Email" )]
        public string Email_Cus { get; set; }

        [StringLength(200)]
        public string Address_Cus { get; set; }

        [StringLength(15)]
        public string Phone_Cus { get; set; }
        public DateTime? CreatedDay { get; set; }
        public double? Score { get; set; }
        public string CustomerRank { get; set; }
        public virtual ICollection<_Order> Orders { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}