using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.Web.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Services
{
    public interface ICatalogViewModelService
    {
        Task<CatalogIndexViewModel> GetCatalogItems(
            int pageIndex,
            int itemsPage,
            string searchText,
            int? brandId,
            int? typeId,
            bool convertPrice = true,
        CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<SelectListItem>> GetBrands(CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<SelectListItem>> GetTypes(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get Item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="convertPrice"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CatalogItemViewModel> GetItemById(int id, bool convertPrice = true, CancellationToken cancellationToken = default(CancellationToken));
    }
}