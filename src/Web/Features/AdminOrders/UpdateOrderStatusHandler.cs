using MediatR;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Features.OrderDetails
{
    public class UpdateOrderStatusHandler : IRequestHandler<GetOrderDetails, OrderViewModel>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderStatusHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderViewModel> Handle(GetOrderDetails request, CancellationToken cancellationToken)
        {
            var customerOrders = await _orderRepository.ListAsync(new OrdersWithStatusSpecification(request.Status));
            var orderStatus = customerOrders.FirstOrDefault(o => o.Status == request.Status);

            if (orderStatus == customerOrders)
            {
                return null;
            }

            return new OrderViewModel()
            {
                Status = orderStatus.Status            
            };
        }
    }
}
