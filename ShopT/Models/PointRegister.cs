using Newtonsoft.Json;
using System;

namespace ShopT.Models
{
    public partial class PointRegister
    {
        [JsonIgnore]
        public int PointRegisterId { get; set; }
        [JsonIgnore]
        public int? OrderId { get; set; }
        public int UserId { get; set; }
        public decimal Points { get; set; }
        public bool UsedOrReceived { get; set; }//true = минус, false = плюс
        public bool TransactionCompleted { get; set; }//При значении ДА возврат средств невозможен
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public virtual Order Order { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
