using ShopT.Models.EnumModels;
using ShopT.Models.LocalModels;
using Xamarin.Forms;

namespace ShopT.ViewModels.Selectors
{
    class OrdersSelector : DataTemplateSelector
    {
        public DataTemplate WaitingOrder { get; set; }
        public DataTemplate Header { get; set; }
        public DataTemplate DeliveredOrder { get; set; }

        public OrdersSelector()
        {
            WaitingOrder = new DataTemplate(typeof(Cell));
            Header = new DataTemplate(typeof(Cell));
            DeliveredOrder = new DataTemplate(typeof(Cell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            OrderLocal order = item as OrderLocal;
            if (order == null)
            {
                return Header;
            }
            if (order.Order.OrderStatus == OrderStatus.delivered)
            {
                return DeliveredOrder;
            }
            else
            {
                return WaitingOrder;
            }
        }
    }
}
