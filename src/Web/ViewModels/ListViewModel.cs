using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.ViewModels
{
    public class ListViewModel
    {
        public IEnumerable<CatalogItemViewModel> Items {get; set;}
      
    }
}