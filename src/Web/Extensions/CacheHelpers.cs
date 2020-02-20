using System;
using System.Text;

namespace Microsoft.eShopWeb.Web.Extensions
{
    public class InvalidPageIndexException : Exception
    {

    }
    public static class CacheHelpers
    {
        public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
        private static readonly string _itemsKeyTemplate = "items-{0}-{1}-{2}-{3}-{4}";

        public static string GenerateCatalogItemCacheKey(int pageIndex, int itemsPage,
            string searchText, int? brandId, int? typeId)
        {
            if (pageIndex < 0)
            {
                throw new InvalidPageIndexException();
            }
            return string.Format(
                _itemsKeyTemplate, pageIndex, itemsPage, brandId, typeId,
                searchText ?? string.Empty.RemoveSpecialCharacters()
            );
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public static string GenerateCatalogItemIdKey(int id)
        {
            return $"catalog_item_{id}";
        }
        public static string GenerateBrandsCacheKey()
        {
            return "brands";
        }

        public static string GenerateTypesCacheKey()
        {
            return "types";
        }
    }
}
