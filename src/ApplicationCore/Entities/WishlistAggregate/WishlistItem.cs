using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.WishlistAggregate
{
    public class WishlistItem : BaseEntity, IAggregateRoot
    {
        public string CatalogItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int CatalogItemId { get; set; }
        public int WishlistId { get; private set; }
    }
}
