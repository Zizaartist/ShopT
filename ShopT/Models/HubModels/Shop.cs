using Newtonsoft.Json;
using ShopT.Models.EnumModels;
using System;
using System.Collections.Generic;


namespace ShopT.Models.HubModels
{
    public partial class Shop
    {
        public int ShopId { get; set; }
        public Status Status { get; set; }
        public string ClientApiAddress { get; set; }
        public string AdminApiAddress { get; set; }
        public int? LocationId { get; set; }
        public int? OwnerId { get; set; }

        public virtual Location Location { get; set; }
        public virtual ShopConfiguration ShopConfiguration { get; set; }
        public virtual ShopInfo ShopInfo { get; set; }
    }
}
