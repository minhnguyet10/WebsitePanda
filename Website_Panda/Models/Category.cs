using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class Category
    {
        [Key]
        public long CategoryID { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Category name is not null! Please enter Image!")]
        public string CategoryName { get; set; }

        [StringLength(100)]
        public string Note { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}