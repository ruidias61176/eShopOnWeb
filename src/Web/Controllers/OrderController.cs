using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.Features.MyOrders;
using Microsoft.eShopWeb.Web.Features.OrderDetails;
using System.Threading;
using System.Threading.Tasks;
using OrderStatus = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.OrderStatus;

namespace Microsoft.eShopWeb.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
    [Route("[controller]/[action]")]
    public class OrderController : Controller, IRequest<Order>
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> MyOrders()
        {
            var viewModel = await _mediator.Send(new GetMyOrders(User.Identity.Name));

            return View(viewModel);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> Detail(int orderId)
        {
            var viewModel = await _mediator.Send(new GetOrderDetails(User.Identity.Name, orderId));

            if (viewModel == null)
            {
                return BadRequest("No such order found for this user.");
            }

            return View(viewModel);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> OrderStatusUpdate(int orderId, OrderStatus newStatus)
        {
            var formerStatus = await _orderRepository.GetByIdAsync(orderId);
            var newOrderStatus = new OrderStatus();
            var viewModel = await _mediator.Send(new UpdateOrderStatus(orderId, newOrderStatus));
            if (viewModel == null)
            {
                return BadRequest();
            }
            await _orderRepository.UpdateAsync(formerStatus);
            return View(newOrderStatus);
        }
    }
}