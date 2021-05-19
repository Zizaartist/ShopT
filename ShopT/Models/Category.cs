using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopT.Models
{
    public partial class Category
    {
        public Category()
        {
            ChildCategories = new HashSet<Category>();
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public int? ParentCategoryId { get; set; }

        [JsonIgnore]
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        [NotMapped]
        public bool IsEndpoint { get; set; }
    }
}
