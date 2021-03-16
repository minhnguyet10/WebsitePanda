using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class BestSeller
    {
        [Key]
        public long IDPro { get; set; }

        [StringLength(250)]
        public string NamePro { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }
        
    }
}