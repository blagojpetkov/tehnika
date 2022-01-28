using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tehnika.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public List<Product> products { get; set; }
        public String username { get; set; }
        public String address { get; set; }
        public ShoppingCart()
        {
            products = new List<Product>();
        }
    }
}