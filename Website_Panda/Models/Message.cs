using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class Message
    {
        [Key]
        public long IDMessage { get; set; }
        [Required(ErrorMessage = "Name is not null! Please enter Name!")]
        public string Name { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public string Subject { get; set; }
        [Required(ErrorMessage = "Message is not null! Please enter Message!")]
        public string MessageContact { get; set; }
        public DateTime? Createday {get;set;}
    }
}