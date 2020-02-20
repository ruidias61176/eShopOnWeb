using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using System.Linq;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities.WishlistAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IAsyncRepository<Wishlist> _wishlistRepository;
        private readonly IAsyncRepository<CatalogItem> _catalogItemRepository;
        private readonly IAsyncRepository<WishlistItem> _wishlistItemRepository;
        private readonly IBasketService _basketService;
        private readonly IAppLogger<WishlistService> _logger;

        public WishlistService(
            IAsyncRepository<Wishlist> wishlistRepository,
            IAsyncRepository<CatalogItem> catalogItemRepository,
            IAsyncRepository<WishlistItem> wishlistItemRepository,
            IBasketService basketService,
            IAppLogger<WishlistService> logger)
        {
            _wishlistRepository = wishlistRepository;
            _wishlistItemRepository= wishlistItemRepository;
            _basketService = basketService;
            _catalogItemRepository = catalogItemRepository;
            _logger = logger;
        }

        public async Task AddWishlistItem(int wishlistId, int catalogItemId, decimal price)
        {
            var wishlist = await _wishlistRepository.GetByIdAsync(wishlistId);
            if (wishlist.Items.Any(x => x.CatalogItemId == catalogItemId)) {
                throw new ItemAlreadyInWishlist("Item already in wishlist!");
            }
            if (wishlistId != wishlist.Id) {
                throw new Exception("Wishlist doen't exist");
            }
            var catalogItem = await _catalogItemRepository.GetByIdAsync(catalogItemId);
            wishlist.AddItemToWishlist(catalogItem.Id, catalogItem.Name, catalogItem.Price);
            _logger.LogInformation($"Wishlist {wishlistId} has a new item {catalogItem.Id} {catalogItem.Name}.");
            await _wishlistRepository.UpdateAsync(wishlist);
        }

        public async Task TransferWishlistItemToBasket(int wishListItemId, int basketId, int catalogItemId, int quantity = 1)
        {
            var wishListItem = await _wishlistItemRepository.GetByIdAsync(wishListItemId);
            if (wishListItem == null) {
                throw new Exception("Wishlist item doen't exist");
            }
            var catalogItem = await _catalogItemRepository.GetByIdAsync(wishListItem.CatalogItemId);
            await _basketService.AddItemToBasket(basketId, wishListItem.CatalogItemId, catalogItem.Price, quantity);
            await _wishlistItemRepository.DeleteAsync(wishListItem);
        }

        public async Task DeleteWishlistAsync(int wishlistId)
        {
            var wishlist = await _wishlistRepository.GetByIdAsync(wishlistId);
            await _wishlistRepository.DeleteAsync(wishlist);
        }

        public async Task<int> GetWishlistItemCountAsync(string userName)
        {
            Guard.Against.NullOrEmpty(userName, nameof(userName));
            var wishlistSpec = new WishlistWithItemsSpecification(userName);
            var wishlist = (await _wishlistRepository.ListAsync(wishlistSpec)).FirstOrDefault();
            if (wishlist == null)
            {
                _logger.LogInformation($"No wishlist found for {userName}");
                return 0;
            }
            int count = wishlist.Items.Count();
            _logger.LogInformation($"Wishlist for {userName} has {count} items.");
            return count;
        }

        public async Task TransferWishlistAsync(string anonymousId, string userName)
        {
            Guard.Against.NullOrEmpty(anonymousId, nameof(anonymousId));
            Guard.Against.NullOrEmpty(userName, nameof(userName));
            var wishlistSpec = new WishlistWithItemsSpecification(anonymousId);
            var wishlist = (await _wishlistRepository.ListAsync(wishlistSpec)).FirstOrDefault();
            if (wishlist == null) return;
            wishlist.BuyerId = userName;
            await _wishlistRepository.UpdateAsync(wishlist);
        }
    }
}
