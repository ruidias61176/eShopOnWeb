using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Pages.Basket;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;

namespace Microsoft.eShopWeb.Web.Pages.Wishlist
{
    public class TransferItemToBasketModel : PageModel
    {
        private readonly IWishlistService _wishlistService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBasketViewModelService _basketViewModelService;
        private readonly IBasketService _basketService;
        private readonly IAsyncRepository<CatalogItem> _catalogItemRepository;
        //TODO:
        //private string _username = null;
        private readonly IWishlistViewModelService _wishlistViewModelService;

        public TransferItemToBasketModel(IWishlistService wishlistService,
            IAsyncRepository<CatalogItem> catalogItemRepository,
            IWishlistViewModelService wishlistViewModelService,
            IBasketViewModelService basketViewModelService,
            IBasketService basketService,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _wishlistService = wishlistService;
            _signInManager = signInManager;
            _catalogItemRepository = catalogItemRepository;
            _wishlistViewModelService = wishlistViewModelService;
            _basketViewModelService = basketViewModelService;
            _basketService = basketService;
        }

        public async Task<IActionResult> OnPost(int wishListItemId)
        {
            var wishlist = await _wishlistViewModelService.GetOrCreateWishlistForUser(User.Identity.Name);
            if (wishlist == null)
            {
                //throw new Exception("Invalid Wishlist");
                return BadRequest("Invalid Wishlist");
            }
            var wishListItem = wishlist.Items.Where(x => x.Id == wishListItemId).FirstOrDefault();
            if (wishListItem == null)
            {
                //throw new Exception("Invalid Wishlist");
                return BadRequest("Item already in Wishlist");
            }

            var basket = await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name);
            if (basket == null)
            {
                return RedirectToPage("/Login");
            }
            var catalogItem = await _catalogItemRepository.GetByIdAsync(wishListItem.CatalogItemId);
            await _basketService.AddItemToBasket(basket.Id, wishListItem.CatalogItemId, catalogItem.Price);
            return RedirectToPage("/Basket/Index");
        }

    }
}
