using System;
using MediatR;
using Microsoft.eShopWeb.Web.ViewModels;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.Web.Features.AdminOrders
{
    public class GetAdminOrders : IRequest<IEnumerable<OrderViewModel>>
    {
        /// <summary>
        /// Date before which an order has been created
        /// </summary>
        public DateTimeOffset? CreatedBefore {get; set;} = null;

        /// <summary>
        /// Date after which an order has been created
        /// </summary>
        public DateTimeOffset? CreatedAfter {get; set;} = null;

        /// <summary>
        /// Identifier of the user making the order
        /// </summary>
        public string BuyerId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="buyerId">Buyer identifier</param>
        /// <param name="createdBefore">Date before which an order has been created</param>
        /// <param name="createdAfter">Date after which an order has been created</param>
        public GetAdminOrders(
            string buyerId = null,
            DateTimeOffset? createdBefore = null,
            DateTimeOffset? createdAfterB = null
        ) {

        }
    }
}
