using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces
{
    public interface IWishlistService
    {
        Task<int> GetWishlistItemCountAsync(string userName);
        Task TransferWishlistAsync(string anonymousId, string userName);
        Task TransferWishlistItemToBasket(int wishListItemId, int basketId, int catalogItemId, int quantity = 1);
        Task AddWishlistItem(int wishlistId, int catalogItemId, decimal price);
        Task DeleteWishlistAsync(int wishlistId);
    }
}
