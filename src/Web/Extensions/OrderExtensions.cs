using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using System.Linq;

namespace Microsoft.eShopWeb.Web.Extensions
{
    public static class OrderExtensions
    {
        public static OrderViewModel CreateViewModel(this Order order) {
            return new OrderViewModel
            {
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemViewModel()
                {
                    PictureUrl = oi.ItemOrdered.PictureUri,
                    ProductId = oi.ItemOrdered.CatalogItemId,
                    ProductName = oi.ItemOrdered.ProductName,
                    UnitPrice = oi.UnitPrice,
                    Units = oi.Units
                }).ToList(),
                OrderNumber = order.Id,
                ShippingAddress = order.ShipToAddress,
                Total = order.Total()
            };
        }

    }
} 