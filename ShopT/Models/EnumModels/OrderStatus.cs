using System.Collections.Generic;

namespace ShopT.Models.EnumModels
{
    public enum OrderStatus 
    {
        sent, //Отправлен клиентом
        received, //Принят брендом (автоматически или через агрегатор)
        onWay, //Исполнитель утверждает что уже доставляет товар
        delivered, //Доставка произведена
    }

    public class OrderStatusDictionaries 
    {
        public static Dictionary<OrderStatus, string> GetStringFromOrderStatus = new Dictionary<OrderStatus, string>()
        {
            { OrderStatus.sent, "Отправлено" },
            { OrderStatus.received, "Принято" },
            { OrderStatus.onWay, "В пути" },
            { OrderStatus.delivered, "Доставлено" }
        };
    }
}
