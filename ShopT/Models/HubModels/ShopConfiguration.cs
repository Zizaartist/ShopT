using Newtonsoft.Json;
using ShopT.Models.EnumModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace ShopT.Models.HubModels
{
    public partial class ShopConfiguration
    {
        public int ShopConfigurationId { get; set; }
        public int ShopId { get; set; }
        public decimal DeliveryPrice { get; set; }
        public int MaxPoints { get; set; }
        public int Cashback { get; set; }
        public decimal? MinimalDeliveryPrice { get; set; }
        public string PaymentMethods { get; set; }
        public long Version { get; set; }

        [JsonIgnore]
        public virtual Shop Shop { get; set; }
        
        [JsonIgnore]
        [NotMapped]
        public List<PaymentMethod> ActualPaymentMethods 
        {
            get
            {
                var result = new List<PaymentMethod>();
                foreach (var character in PaymentMethods)
                    result.Add((PaymentMethod)int.Parse(character.ToString()));
                return result;
            }
            set 
            {
                var result = "";
                foreach (var pm in value)
                    result += (int)pm;
                PaymentMethods = result;
            }
        }
    }
}
