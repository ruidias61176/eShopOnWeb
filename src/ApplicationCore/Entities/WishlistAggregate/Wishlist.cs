using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.WishlistAggregate
{
    public class Wishlist : BaseEntity, IAggregateRoot
    {
        public string WishlistName {get; set;}
        public string BuyerId { get; set; }
        private readonly List<WishlistItem> _items = new List<WishlistItem>();
        public IReadOnlyCollection<WishlistItem> Items => _items.AsReadOnly();

        public void AddItemToWishlist(int catalogItemId, string catalogItemName, decimal unitPrice)
        {
            try {
                var existingItem = Items.FirstOrDefault(i => i.CatalogItemId == catalogItemId);
                throw new ItemAlreadyInWishlist(catalogItemName);
            }
            catch
            {if (!Items.Any(i => i.CatalogItemId == catalogItemId))
            {
                _items.Add(new WishlistItem()
                {
                    CatalogItemName = catalogItemName,
                    CatalogItemId = catalogItemId,
                    UnitPrice = unitPrice
                });
                return;
            }}
        }

        public void RemoveWishlistItem(int catalogItemId)
        {
            _items.RemoveAll(x => x.CatalogItemId == catalogItemId);
        }
    }
}
