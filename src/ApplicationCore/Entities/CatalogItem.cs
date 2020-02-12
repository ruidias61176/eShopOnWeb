using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class CatalogItem : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUri { get; set; }
        public int CatalogTypeId { get; set; }
        public int CatalogBrandId { get; set; }
        public bool ShowPrice {get; set;} = true;
        public int Stock {get;set;}
        public string StoreName {get;set;}
        

        #region "Navigation properties"
        public CatalogBrand CatalogBrand { get; set; }
        public CatalogType CatalogType { get; set; }
        //public CatalogStore CatalogStore {get; set; }

        #endregion

       
    }
}