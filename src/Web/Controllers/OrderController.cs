using ApplicationCore.Entities.OrderAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.Features.MyOrders;
using Microsoft.eShopWeb.Web.Features.OrderDetails;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Controllers
{
    public class OrderToUpdate : IRequest<Order>
    {
        public int OrderId { get; set; }
        public OrderStatus StatusToUpdate { get; set; }
    }

    public class OrderToDelete : IRequest
    {
        public int OrderId { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
    [Route("[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;
        private readonly IStringLocalizer<OrderController> _stringLocalizer;

        public OrderController(IMediator mediator, IOrderRepository orderRepository, IStringLocalizer<OrderController> stringLocalizer)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
            _stringLocalizer = stringLocalizer;
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
            var viewModel = await _mediator.Send(new GetOrderDetails(User.Identity.Name, orderId, OrderStatus.Pending));

            if (viewModel == null)
            {
                return BadRequest("No such order found for this user.");
            }

            return View(viewModel);
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(OrderToUpdate request, CancellationToken cancellationToken)
        {
            var viewModel = await _mediator.Send((new UpdateOrderStatus(request.OrderId, request.StatusToUpdate)));
            var order = request.OrderId;
            if (order != viewModel.OrderNumber)
            {
                throw new Exception("Orders Ids do not match");
            }
            var updatedOrder = new OrderStatus();
            updatedOrder = request.StatusToUpdate;
            //TODO:
            //await _mediator.Send(updatedOrder, cancellationToken);
            return View(updatedOrder);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(OrderToDelete request)
        {
            await _mediator.Send(request);
            return Ok();
        }

    }
}