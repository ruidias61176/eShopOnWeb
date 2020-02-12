using Microsoft.eShopWeb.ApplicationCore.Entities.WishlistAggregate;

namespace Microsoft.eShopWeb.UnitTests.Builders
{
    public class WishlistBuilder
    {
        private Wishlist _wishlist;
        public string WishlistBuyerId => "testbuyerId@test.com";
        public int WishlistId => 1;

        public WishlistBuilder()
        {
            _wishlist = WithNoItems();
        }

        public Wishlist Build()
        {
            return _wishlist;
        }

        public Wishlist WithNoItems()
        {
            _wishlist = new Wishlist { BuyerId = WishlistBuyerId, Id = WishlistId };
            return _wishlist;
        }

        public Wishlist WithOneBasketItem()
        {
            _wishlist = new Wishlist { BuyerId = WishlistBuyerId, Id = WishlistId };
            _wishlist.AddItemToWishlist(2, "string", 4);
            return _wishlist;
        }
    }
}
