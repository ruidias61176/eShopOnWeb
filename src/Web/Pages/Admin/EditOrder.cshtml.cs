using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Pages.Admin
{
    [Authorize(Roles = AuthorizationConstants.Roles.ADMINISTRATORS)]
    public class EditOrderModel : PageModel
    {
        private readonly IOrderViewModelService _orderViewModelService;

        public EditOrderModel(IOrderViewModelService orderViewModelService)
        {
            _orderViewModelService = orderViewModelService;
        }
        
       [BindProperty]
        public OrderViewModel OrderModel { get; set; } = new OrderViewModel();

        public Task OnGet(OrderViewModel orderModel)
        {
            OrderModel = orderModel;
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _orderViewModelService.UpdateOrder(OrderModel);
            }

            return RedirectToPage("/Admin/Index");
        }
    }
}