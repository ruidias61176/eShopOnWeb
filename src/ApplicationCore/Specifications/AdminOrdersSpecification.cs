using System;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Helpers.Query;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications
{
    public class AdminOrdersSpecification : BaseSpecification<Order>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyerId">Buyer identifier</param>
        /// <param name="createdBefore">Date up until which an order has been created</param>
        /// <param name="createdAfter">Date after which an order has been created</param>
        public AdminOrdersSpecification(
            string buyerId = null,
            DateTimeOffset? createdBefore = null,
            DateTimeOffset? createdAfter = null)
            : base(order => (string.IsNullOrEmpty(buyerId) || order.BuyerId == buyerId) &&
            (!createdBefore.HasValue || order.OrderDate >= createdBefore.Value) &&
            (!createdAfter.HasValue || order.OrderDate <= createdAfter.Value))
        {
            AddIncludes(query => query.Include(o => o.OrderItems).ThenInclude(i => i.ItemOrdered));
        }
    }
}
