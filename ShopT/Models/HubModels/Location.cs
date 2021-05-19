using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace ShopT.Models.HubModels
{
    public partial class Location
    {
        public Location()
        {
            Shops = new HashSet<Shop>();
        }

        public int LocationId { get; set; }
        public string LocationName { get; set; }

        [JsonIgnore]
        public virtual ICollection<Shop> Shops { get; set; }
    }
}
