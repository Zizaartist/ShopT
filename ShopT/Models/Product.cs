using Newtonsoft.Json;
using ShopT.Models.Statistics;
using System;
using System.Collections.Generic;

namespace ShopT.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Reports = new HashSet<Report>();
        }

        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int? Discount { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int InStorage { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<Report> Reports { get; set; }

        public string CategoryName { get; set; }
    }
}
