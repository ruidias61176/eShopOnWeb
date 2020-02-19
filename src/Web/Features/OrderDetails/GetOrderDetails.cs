using ApplicationCore.Entities.OrderAggregate;
using MediatR;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Features.OrderDetails
{
    public class GetOrderDetails : IRequest<OrderViewModel>
    {
        public string UserName { get; set; }
        public int OrderId { get; set; }

        public OrderStatus Status { get; set; }

        public GetOrderDetails(string userName, int orderId, OrderStatus status)
        {
            UserName = userName;
            OrderId = orderId;
            Status = status;
        }
    }
}
