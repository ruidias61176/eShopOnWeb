using Microsoft.eShopWeb.Web.ViewModels;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Interfaces
{
    public interface IOrderViewModelService
    {
        Task UpdateOrder(OrderViewModel viewModel);
    }
}