using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShopT.Models
{
    public partial class User
    {
        public User()
        {
            ErrorReports = new HashSet<ErrorReport>();
            Orders = new HashSet<Order>();
            PointRegisters = new HashSet<PointRegister>();
        }

        [JsonIgnore]
        public int UserId { get; set; }
        public string Phone { get; set; }
        [JsonIgnore]
        public decimal Points { get; set; }
        [JsonIgnore]
        public bool NotificationsEnabled { get; set; }
        [JsonIgnore]
        public string NotificationRegistration { get; set; }
        [JsonIgnore]
        public string DeviceType { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<ErrorReport> ErrorReports { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
        [JsonIgnore]
        public virtual ICollection<PointRegister> PointRegisters { get; set; }
    }
}
