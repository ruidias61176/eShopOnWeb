using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Services
{
    public class CatalogItemViewModelService : ICatalogItemViewModelService
    {
        private readonly IAsyncRepository<CatalogItem> _catalogItemRepository;

        public CatalogItemViewModelService(IAsyncRepository<CatalogItem> catalogItemRepository)
        {
            _catalogItemRepository = catalogItemRepository;
        }

        public Task<IEnumerable<SelectListItem>> GetBrands(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<SelectListItem>> GetTypes(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateCatalogItem(CatalogItemViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            //Get existing CatalogItem
            var existingCatalogItem = await _catalogItemRepository.GetByIdAsync(viewModel.Id);

            //Build updated CatalogItem
            var updatedCatalogItem = existingCatalogItem;
            updatedCatalogItem.Name = viewModel.Name;
            updatedCatalogItem.Price = viewModel.Price;

            await _catalogItemRepository.UpdateAsync(updatedCatalogItem);
        }
    }
}