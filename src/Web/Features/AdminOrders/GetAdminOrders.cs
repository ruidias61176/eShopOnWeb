using System;
using MediatR;
using Microsoft.eShopWeb.Web.ViewModels;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.Web.Features.AdminOrders
{
    public class GetAdminOrders : IRequest<IEnumerable<OrderViewModel>>
    {
        public DateTimeOffset? CreatedBefore {get; set;} = null;

        public DateTimeOffset? CreatedAfter {get; set;} = null;

        public string BuyerId { get; set; }

        public GetAdminOrders(
            string buyerId = null,
            DateTimeOffset? createdBefore = null,
            DateTimeOffset? createdAfterB = null
        ) {

        }
    }
}