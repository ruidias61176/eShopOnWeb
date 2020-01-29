﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Services
{
    /// <summary>
    /// This is a UI-specific service so belongs in UI project. It does not contain any business logic and works
    /// with UI-specific types (view models and SelectListItem types).
    /// </summary>
    public class CatalogViewModelService : ICatalogViewModelService
    {
        private readonly ILogger<CatalogViewModelService> _logger;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;
        private readonly IAsyncRepository<CatalogBrand> _brandRepository;
        private readonly IAsyncRepository<CatalogType> _typeRepository;
        private readonly IUriComposer _uriComposer;
        private readonly ICurrencyService _currencyService;
        private const Currency DEFAULT_PRICE_UNIT = Currency.USD; // TODO: Get from Configuration
        private const Currency USER_PRICE_UNIT = Currency.EUR; // TODO: Get from IUserCurrencyService    
        public CatalogViewModelService(
            ILoggerFactory loggerFactory,
            IAsyncRepository<CatalogItem> itemRepository,
            IAsyncRepository<CatalogBrand> brandRepository,
            IAsyncRepository<CatalogType> typeRepository,
            IUriComposer uriComposer,
            ICurrencyService currencyService
        )
        {
            _logger = loggerFactory.CreateLogger<CatalogViewModelService>();
            _itemRepository = itemRepository;
            _brandRepository = brandRepository;
            _typeRepository = typeRepository;
            _uriComposer = uriComposer;
            _currencyService = currencyService;
        }

        /// <summary>
        /// Create a catalog item view model from a catalog item data model
        /// </summary>
        /// <param name="catalogItem">Catalog item</param>
        /// <returns>CatalogItemViewModel</returns>
        private async Task<CatalogItemViewModel> CreateCatalogItemViewModelAsync(
            CatalogItem catalogItem, CancellationToken cancellationToken = default(CancellationToken)) {
            return new CatalogItemViewModel()
                {
                    Id = catalogItem.Id,
                    Name = catalogItem.Name,
                    PictureUri = catalogItem.PictureUri,
                    Price = await _currencyService.Convert(
                        catalogItem.Price, DEFAULT_PRICE_UNIT, USER_PRICE_UNIT, cancellationToken),
                    ShowPrice = catalogItem.ShowPrice,
                    PriceUnit  = USER_PRICE_UNIT
                };
        }

        public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("GetCatalogItems called.");

            var filterSpecification = new CatalogFilterSpecification(brandId, typeId);
            var filterPaginatedSpecification =
                new CatalogFilterPaginatedSpecification(itemsPage * pageIndex, itemsPage, brandId, typeId);

            // the implementation below using ForEach and Count. We need a List.
            var itemsOnPage = await _itemRepository.ListAsync(filterPaginatedSpecification);
            var totalItems = await _itemRepository.CountAsync(filterSpecification);

            foreach (var itemOnPage in itemsOnPage)
            {
                itemOnPage.PictureUri = _uriComposer.ComposePicUri(itemOnPage.PictureUri);
            }
            var CatalogItemsTask = Task.WhenAll(itemsOnPage.Select(
                catalogItem => CreateCatalogItemViewModelAsync(catalogItem, cancellationToken)));
            cancellationToken.ThrowIfCancellationRequested();
            var vm = new CatalogIndexViewModel()
            {
                CatalogItems = await CatalogItemsTask, // catalogItemsList,
                Brands = await GetBrands(),
                Types = await GetTypes(),
                BrandFilterApplied = brandId ?? 0,
                TypesFilterApplied = typeId ?? 0,
                PaginationInfo = new PaginationInfoViewModel()
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = itemsOnPage.Count,
                    TotalItems = totalItems,
                    TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems / itemsPage)).ToString())
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return vm;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands(CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("GetBrands called.");
            var brands = await _brandRepository.ListAllAsync();

            var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
            foreach (CatalogBrand brand in brands)
            {
                items.Add(new SelectListItem() { Value = brand.Id.ToString(), Text = brand.Brand });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes(CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("GetTypes called.");
            var types = await _typeRepository.ListAllAsync();
            var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
            foreach (CatalogType type in types)
            {
                items.Add(new SelectListItem() { Value = type.Id.ToString(), Text = type.Type });
            }

            return items;
        }

        public async Task<CatalogItemViewModel> GetItemById(int id, CancellationToken cancellationToken = default)
        {
            try {
                var item = await _itemRepository.GetByIdAsync(id);
                if (item == null) {
                    throw new ModelNotFoundException($"Catalog item not found. id={id}");
                }
                var catalogItemViewModel = await CreateCatalogItemViewModelAsync(
                    item, cancellationToken);
                return catalogItemViewModel;
            } catch (Exception ex) {
                throw new ModelNotFoundException($"Catalog item not found. id={id}", ex);
            }
        }
    }

    public class ModelNotFoundException: Exception {

        public ModelNotFoundException(string message, Exception innerException = null)
            : base(message, innerException) {

            }
    }
}