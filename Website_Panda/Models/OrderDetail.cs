using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class OrderDetail
    {
        [Key]
        public long IDOrderDetail { get; set; }

        [ForeignKey("Order")]
        public long IDOrder { get; set; }
        public virtual _Order Order { get; set; }

        [ForeignKey("Product")]
        public long IDProduct { get; set; }
        public virtual Product Product { get; set; }

        public decimal? Price { get; set; }
        public int? QuantitySale { get; set; }
        public decimal? OrderTotalPrice { get; set; }
        public DateTime? CreateDay { get; set; }

        
    }
}