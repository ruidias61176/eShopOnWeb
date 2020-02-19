using MediatR;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.eShopWeb.Web.Extensions;

namespace Microsoft.eShopWeb.Web.Features.AdminOrders
{
    public class GetAdminOrdersHandler : IRequestHandler<GetAdminOrders, IEnumerable<OrderViewModel>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAdminOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderViewModel>> Handle(GetAdminOrders request, CancellationToken cancellationToken)
        {
            var specification = new AdminOrdersSpecification(
                request.BuyerId,
                request.CreatedBefore,
                request.CreatedAfter);
            var orders = await _orderRepository.ListAsync(specification);

            return orders.Select(order => order.CreateViewModel());
        }
    }
}
