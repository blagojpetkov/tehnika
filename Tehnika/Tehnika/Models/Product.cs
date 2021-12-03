using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tehnika.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Display(Name="Име")]
        public string Name { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Изглед")]
        public string ImageURL { get; set; }
        [Display(Name = "Цена")]
        public double Price { get; set; }
        [Display(Name = "Попуст")]
        public double Discount { get; set; }
        [Display(Name = "Гаранција")]
        public int Warranty { get; set; }
        [Display(Name = "Категорија")]
        public string Category { get; set; }

    }
}