using Newtonsoft.Json;

namespace ShopT.Models
{
    public partial class OrderInfo
    {
        [JsonIgnore]
        public int OrderInfoId { get; set; }
        [JsonIgnore]
        public int OrderId { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string OrdererName { get; set; }
        public string House { get; set; }
        public int? Entrance { get; set; }
        public int? Floor { get; set; }
        public int? Apartment { get; set; }
        public string Commentary { get; set; }

        [JsonIgnore]
        public virtual Order Order { get; set; }
    }
}
