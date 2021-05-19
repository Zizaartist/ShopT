using Newtonsoft.Json;

namespace ShopT.Models
{
    public partial class OrderDetail
    {
        [JsonIgnore]
        public int OrderDetailId { get; set; }
        public int Count { get; set; }
        public int? ProductId { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        public int OrderId { get; set; }

        [JsonIgnore]
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
