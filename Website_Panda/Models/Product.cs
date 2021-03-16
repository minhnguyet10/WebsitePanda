using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class Product
    {
        [Key]
        public long IDProduct { get; set; }
        [Required(ErrorMessage = "Product name is not null! Please enter Product name!")]
        [StringLength(200)]
        public string ProductName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public string Information { get; set; }
        [Required(ErrorMessage = "Image product is not null!")]
        public string Image { get; set; }
        public string MoreImage1 { get; set; }
        public string MoreImage2 { get; set; }
        [Required(ErrorMessage = "Price is not null! Please enter Price!")]
        //[GreaterThan("PromotionPrice", ErrorMessage = "PromotionPrice < Price !!!")]
        public decimal? Price { get; set; }
        
        public decimal? PromotionPrice { get; set; }
        public int? Quantity { get; set; }
        public string Color { get; set; }

        [ForeignKey("Category")]
        public long? CategoryID { get; set; }
        public virtual Category Category { get; set; }
        
        [ForeignKey("Brand")]
        public long? BrandID { get; set; }
        public virtual Brand Brand { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}