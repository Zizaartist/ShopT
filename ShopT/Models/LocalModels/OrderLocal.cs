using ShopT.Models.EnumModels;
using System.Collections.Generic;

namespace ShopT.Models.LocalModels
{
    public class OrderLocal
    {
        public OrderLocal(Order _order)
        {
            Order = _order;

            var temp = new List<string>();
            if (Order.OrderInfo.Street != null) temp.Add(Order.OrderInfo.Street);
            if (Order.OrderInfo.Apartment != null) temp.Add($"кв. {Order.OrderInfo.Apartment}");
            if (Order.OrderInfo.Entrance != null) temp.Add($"подъезд {Order.OrderInfo.Entrance}");
            if (Order.OrderInfo.Floor != null) temp.Add($"этаж {Order.OrderInfo.Floor}");
            var addressParts = temp.ToArray();
            Address = string.Join(", ", addressParts);

            Delivered = Order.OrderStatus == OrderStatus.delivered;

            Status = OrderStatusDictionaries.GetStringFromOrderStatus[Order.OrderStatus];

            if (Order.DeliveryPrice == null) TakeoutDelivery = "Самовывоз";
        }

        public Order Order { get; private set; }
        public string Address { get; private set; }
        public bool Delivered { get; private set; }
        public string Status { get; private set; }
        public string TakeoutDelivery { get; private set; } = "Адрес доставки: "; //Определяет текст лейбла
    }
}
