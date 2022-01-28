using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tehnika.Models
{
    public class ProductComment
    {
        public int Id { get; set; }
        public ApplicationUser user { get; set; }
        public string comment { get; set; }
    }
}