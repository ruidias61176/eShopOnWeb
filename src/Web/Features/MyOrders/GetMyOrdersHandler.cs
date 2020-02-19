using MediatR;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.Extensions;
using Microsoft.eShopWeb.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Features.MyOrders
{
    public class GetMyOrdersHandler : IRequestHandler<GetMyOrders, IEnumerable<OrderViewModel>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetMyOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderViewModel>> Handle(GetMyOrders request, CancellationToken cancellationToken)
        {
            var specification = new CustomerOrdersWithItemsSpecification(request.UserName);
            var orders = await _orderRepository.ListAsync(specification);
            return orders.Select(order => order.CreateViewModel());
        }
    }
}
