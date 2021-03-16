using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class IncomeMD
    {
        [Key]
        public long? IDOrder { get; set; }
        public DateTime? OrderDate { get; set; }
        public long? IdCus { get; set; }
        public decimal? Price { get; set; }
        public int? QuantitySale { get; set; }
        public int? TotalOrder { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}