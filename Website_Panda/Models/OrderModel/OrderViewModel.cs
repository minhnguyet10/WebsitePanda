using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_Panda.Models.OrderModel
{
    public class OrderViewModel
    {
        [Key]
        public long IDOrder { get; set; }
        public DateTime? OrderDate { get; set; }

        [StringLength(500)]
        public string Descriptions { get; set; }
        public long? IdCus { get; set; } 

        [StringLength(150)]
        public string Email_Cus { get; set; }

        [StringLength(50)]
        public string Address_cus { get; set; }

        [StringLength(25)]
        public string Phone_Cus { get; set; }
        public bool? Status { get; set; }
        public bool? Paid { get; set; }
        public string Cancelled { get; set; }
        public bool? Delivered { get; set; }
        public DateTime? ComfirmDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public virtual Customer Customer  { get; set; }
    }
}