﻿using Newtonsoft.Json;
using ShopT.Models.EnumModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ShopT.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            PointRegisters = new HashSet<PointRegister>();
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ChangeBanknote? ChangeBanknote { get; set; }
        public bool PointsUsed { get; set; }
        public decimal Sum { get; set; }
        public decimal? DeliveryPrice { get; set; }
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
        public virtual OrderInfo OrderInfo { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<PointRegister> PointRegisters { get; set; }
        [NotMapped]
        public bool Delivery { get; set; } //Получаем от клиента
        [NotMapped]
        [JsonIgnore]
        public PointRegister PointRegister
        {
            get
            {
                return PointRegisters?.FirstOrDefault(pr => pr.UsedOrReceived);
            }
        }
        [NotMapped]
        public string OrdererName { get; set; }
    }
}
