using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        [ForeignKey("Customer")]
        public long IdCus { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("Product")]
        public long IDProduct { get; set; }
        public virtual Product Product { get; set; }

        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public int? CartTotalQuantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? CartTotalPrice { get; set; }
    }
}