using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace ApplicationCore.Entities.OrderAggregate
{
public enum OrderStatus 
    {
        Pending,
        Processing,
        Dispatched,
        Complete,
        Cancelled
    }

}
    
   