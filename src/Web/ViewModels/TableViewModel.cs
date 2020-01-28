using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.ViewModels
{
    public class TableViewModel
    {
        public IEnumerable<CatalogItemViewModel> Items {get; set;}
         public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Currency PriceUnit {get; set;}
    }
}