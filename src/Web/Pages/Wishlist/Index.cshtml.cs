﻿using Microsoft.AspNetCore.Http;
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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Pages.Wishlist
{
    public class IndexModel : PageModel
    {
        private readonly IWishlistService _wishlistService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBasketViewModelService _basketViewModelService;
        private string _username = null;
        private readonly IWishlistViewModelService _wishlistViewModelService;

        public IndexModel(IWishlistService wishlistService,
            IWishlistViewModelService wishlistViewModelService, 
            IBasketViewModelService basketViewModelService,
            SignInManager<ApplicationUser> signInManager)
        {
            _wishlistService = wishlistService;
            _signInManager = signInManager;
            _wishlistViewModelService = wishlistViewModelService;
            _basketViewModelService = basketViewModelService;
        }

        public WishlistViewModel WishlistViewModel { get; set; } = new WishlistViewModel();
        public BasketViewModel BasketModel { get; set; } = new BasketViewModel();

        public async Task OnGet()
        {
            await SetWishlistModelAsync();
        }

        public async Task<IActionResult> OnPost(CatalogItemViewModel productDetails)
        {
            if (productDetails?.Id == null)
            {
                return RedirectToPage("/Index");
            }
            await SetWishlistModelAsync();

            await _wishlistService.AddWishlistItem(WishlistViewModel.Id, productDetails.Id, productDetails.Price);

            await SetWishlistModelAsync();

            return RedirectToPage();
        }

        private async Task SetWishlistModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                WishlistViewModel = await _wishlistViewModelService.GetOrCreateWishlistForUser(User.Identity.Name);
                BasketModel  = await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name);
            }
            else
            {
                GetOrSetWishlistCookieAndUserName();
                WishlistViewModel = await _wishlistViewModelService.GetOrCreateWishlistForUser(_username);
                BasketModel  = await _basketViewModelService.GetOrCreateBasketForUser(_username);
            }
        }

        private void GetOrSetWishlistCookieAndUserName()
        {
            if (Request.Cookies.ContainsKey(Constants.WISHLIST_COOKIENAME))
            {
                _username = Request.Cookies[Constants.WISHLIST_COOKIENAME];
            }
            if (_username != null) return;

            _username = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true };
            cookieOptions.Expires = DateTime.Today.AddYears(10);
            Response.Cookies.Append(Constants.WISHLIST_COOKIENAME, _username, cookieOptions);
        }
    }
}
