using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class Banner
    {
        [Key]
        public int IDBanner { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Image is not null! Please enter Image!")]
        public string Image { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}