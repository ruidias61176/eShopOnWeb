using System.Runtime.Serialization;
using ApplicationCore.Entities.OrderAggregate;
using MediatR;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Features.OrderDetails
{
    [DataContract]
    public class UpdateOrderStatus : IRequest<OrderViewModel>
    {
        [DataMember]
        public OrderStatus Status { get; private set; }
        [DataMember]
        public int OrderId { get; set; }

        public UpdateOrderStatus(int orderId, OrderStatus status)
        {
            Status = status;
            OrderId = orderId;
        }
    }
    }
