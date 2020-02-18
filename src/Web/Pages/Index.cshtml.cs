using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.Web.Extensions;

namespace Microsoft.eShopWeb.Web.Pages {
    public class IndexModel : PageModel {
        private readonly ICatalogViewModelService _catalogViewModelService;
        private readonly IStringLocalizer<IndexModel> _stringLocalizer;
        private readonly IHtmlLocalizer<IndexModel> _htmlLocalizer;

        public IndexModel(ICatalogViewModelService catalogViewModelService,             IStringLocalizer<IndexModel> stringLocalizer,
            IHtmlLocalizer<IndexModel> htmlLocalizer) {
            _catalogViewModelService = catalogViewModelService;
            _stringLocalizer = stringLocalizer;
            _htmlLocalizer = htmlLocalizer;
        }

        public CatalogIndexViewModel CatalogModel { get; set; } = new CatalogIndexViewModel();

        public async Task OnGet(CatalogIndexViewModel catalogModel, int? pageId) {
            CatalogModel = await _catalogViewModelService.GetCatalogItems(
                pageId ?? 0, 
                Constants.ITEMS_PER_PAGE,
                catalogModel.SearchText,
                catalogModel.BrandFilterApplied, 
                catalogModel.TypesFilterApplied, 
                convertPrice: true,
                HttpContext.RequestAborted);
            CatalogModel.ResultView = catalogModel.ResultView; // HACK
            CatalogModel.ResultViews = Enum<ResultView>.GetAll()
                .Select(resultView => new SelectListItem { Value = resultView.ToString(), Text = resultView.ToString() });
        }
    }
}