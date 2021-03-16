using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class PaymentMethod
    {
        [Key]
        public int IDMethod { get; set; }
        public string MethodName { get; set; }
        public virtual ICollection<_Order> Orders { get; set; }
    }
}