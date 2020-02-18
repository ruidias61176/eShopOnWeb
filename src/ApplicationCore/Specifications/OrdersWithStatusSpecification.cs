using ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using static Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Order;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications
{
    public class OrdersWithStatusSpecification : BaseSpecification<Order>
    {
        public OrdersWithStatusSpecification(OrderStatus? orderStatus)
            :base(order => !orderStatus.HasValue || order.Status == orderStatus.Value)
        {
        }
    }
}
