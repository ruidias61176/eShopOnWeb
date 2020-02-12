using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class CatalogStore : BaseEntity, IAggregateRoot
    {
        public string StoreName { get; set; }
    
    }
}
