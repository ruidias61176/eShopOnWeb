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
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.Controllers.Api;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.Web.Controllers
{
    public class OrderToUpdate : BaseApiController, IRequest<Order>
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
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IStringLocalizer<OrderController> _stringLocalizer;
        private readonly IAppLogger<OrderController> _logger;

        public OrderController(IMediator mediator, IAsyncRepository<Order> orderRepository, IStringLocalizer<OrderController> stringLocalizer, IAppLogger<OrderController> logger)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
            _stringLocalizer = stringLocalizer;
            _logger = logger;
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
        [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
        [HttpPut("{orderId}")]
        public async Task<ActionResult<OrderStatus>> UpdateOrder(int orderId, OrderToUpdate request, EqualityComparer<OrderStatus> comparer, CancellationToken cancellationToken)
        {
            try
            {
                var orderToUpdate = await _orderRepository.GetByIdAsync(orderId);
                var updatedOrder = Comparer<OrderStatus>.Default;
                updatedOrder.Compare(orderToUpdate.Status, request.StatusToUpdate);
                await _orderRepository.UpdateAsync(orderToUpdate);
                _logger.LogInformation($"Order {request.OrderId} status has been updated to {request.StatusToUpdate}");
                return Ok(request);
            }
            catch (ModelNotFoundException orderStatusError)
            {
                return BadRequest(orderStatusError);
            }
        }
    
    [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(OrderToDelete request)
    {
        await _mediator.Send(request);
        _logger.LogInformation($"Order {request.OrderId} status has been deleted");
        return Ok();
    }

}
}