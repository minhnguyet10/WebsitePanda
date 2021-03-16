using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class ForgotPassword
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "Username is not null!")]
        public string Username { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}