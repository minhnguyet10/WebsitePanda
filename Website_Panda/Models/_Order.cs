using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class _Order
    {
        [Key]
        public long IDOrder { get; set; }
        public decimal? Totalpay { get; set; }
        public DateTime? OrderDate { get; set; }
        
        [StringLength(500)]
        public string Descriptions { get; set; }
        
        [ForeignKey("Customer")]
        public long? IdCus { get; set; }
        public virtual Customer Customer { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }
        [StringLength(150)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email")]
        public string Email_Cus { get; set; }

        [StringLength(50)]
        public string Address_cus { get; set; }

        [StringLength(25)]
        public string Phone_Cus { get; set; }
        public bool? Status { get; set; }
        public bool? Paid { get; set; }
        public bool? Cancelled { get; set; }
        public bool? Delivered { get; set; }
        public DateTime? ComfirmDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        [ForeignKey("PaymentMethod")]
        public int? IDMethod { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}