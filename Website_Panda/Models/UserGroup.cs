using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website_Panda.Models
{
    public class UserGroup
    {
        [Key]
        [StringLength(20)]
        public string GroupID { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        [StringLength(100)]
        public string Note { get; set; }
    }
}