using System;
using System.Linq;
using Microsoft.eShopWeb.ApplicationCore.Entities.WishlistAggregate;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.WishlistTests
{
    public class WishlistTest
    {
        private int _catalogItemId = 21;
        private string _catalogItemName = "Ihate.net t-shirt";
        private decimal _unitPrice = 6.54m;
        
    
        [Fact]
        public void AddsWishListItemIfNotPresent()
        {
            var wishList = new Wishlist();
            wishList.AddItemToWishlist(_catalogItemId, _catalogItemName, _unitPrice);

            var firstItem = wishList.Items.Single();
            Assert.Equal(_catalogItemId, firstItem.CatalogItemId);
            Assert.Equal(_catalogItemName, firstItem.CatalogItemName);
            Assert.Equal(_unitPrice, firstItem.UnitPrice);
        }

    }
} 