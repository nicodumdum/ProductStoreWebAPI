using ProductStore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductStore.Models
{
    public class Product
    {
        public int Id { get; set; } //==> No need for [Required] annotation since product id is auto-incremented in the database
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        //[Required]
        [NullableRequired]
        public decimal? Price { get; set; }
    }
}