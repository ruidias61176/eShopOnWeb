using System.Collections.Generic;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.ViewModels
{
    public class GridViewModel
    {
        public IEnumerable<CatalogItemViewModel> Items { get; set; }
        public int NumColumns { get; set; }
    }
}