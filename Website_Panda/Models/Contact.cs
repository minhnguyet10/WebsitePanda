using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class Contact
    {
        [Key]
        public long IDContact { get; set; }

        [StringLength(20)]
        public string ShopName { get; set; }

        [StringLength(200)]
        public string ShopAddress { get; set; }
        
        [StringLength(15)]
        public string ShopPhone { get; set; }

        [StringLength(50)]
        public string ShopEmail { get; set; }
        public string Description { get; set; }
    }
}