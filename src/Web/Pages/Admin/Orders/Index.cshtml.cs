using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.eShopWeb.Web.Features.AdminOrders;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Pages.Admin.Orders
{
    [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();

        public async Task OnGet(string buyerId = null,
            DateTimeOffset? createdBefore = null,
            DateTimeOffset? createdAfterB = null)
        {
            Orders = await _mediator.Send(new GetAdminOrders());
        }
    }
}