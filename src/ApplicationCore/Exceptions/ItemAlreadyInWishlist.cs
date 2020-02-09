using System;

namespace Microsoft.eShopWeb.ApplicationCore.Exceptions
{
    public class ItemAlreadyInWishlist : Exception
    {
        public ItemAlreadyInWishlist(string CatalogItemName) : base($"You'a already added {CatalogItemName} to your Wishlist! Maybe, you should buy it!")
        {
        }
        protected ItemAlreadyInWishlist(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
        public ItemAlreadyInWishlist(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
