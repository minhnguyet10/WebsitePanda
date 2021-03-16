using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class Blog
    {
        [Key]
        public int IDBlog { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Image is not null! Please enter Image!")]
        public string Image { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Subtitle { get; set; }

        public string Content { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}