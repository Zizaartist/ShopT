using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace ShopT.Models.HubModels
{ 
    public partial class ShopInfo
    {
        public int ShopInfoId { get; set; }
        public int ShopId { get; set; }
        public string OrganizationName { get; set; }
        public string ActualAddress { get; set; }
        public string Avatar { get; set; }
        public string Banner { get; set; }
        public string DeliveryTerms { get; set; }
        public string Email { get; set; }
        public string Phones { get; set; }
        public string Inn { get; set; }
        public string Ogrnip { get; set; }
        public string LegalAddress { get; set; }

        [JsonIgnore]
        public virtual Shop Shop { get; set; }
    }
}
