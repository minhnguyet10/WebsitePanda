using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Website_Panda.Models
{
    public class Brand
    {
        [Key]
        public long BrandID { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Brand name is not null!")]
        public string BrandName { get; set; }

        [StringLength(100)]
        public string Note { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}