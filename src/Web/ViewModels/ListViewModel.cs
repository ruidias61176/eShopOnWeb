using System;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.Web.ViewModels
{
    public class ListViewModel
    {
        public IEnumerable<CatalogItemViewModel> Items {get; set;}
        public Func<dynamic, object> ItemTemplate {get; set;}
    }
}