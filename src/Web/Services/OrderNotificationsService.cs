using ApplicationCore.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scriban;
using Scriban.Runtime;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Services

{
    public class OrderNotificationsService
    {
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private IServiceProvider _serviceProvider;
        public OrderNotificationsService(IAsyncRepository<Order> orderRepository, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IServiceProvider serviceProvider)
        {
            _orderRepository = orderRepository;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _serviceProvider = serviceProvider;

            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            var TemplateSubject = configuration.GetValue<string>("SendGrid:templateSubject");
            var TemplateBody = configuration.GetValue<string>("SendGrid:templateBody");
        }

        public async Task OrderStatusNotifyAsync(int orderId, OrderStatus newOrderStatus)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(orderId);
            var users = _signInManager.UserManager.Users.ToList();
            if (newOrderStatus != existingOrder.Status)
            {
                await OrderStatusNotify(existingOrder.BuyerId, existingOrder, newOrderStatus);
            }
        }

        private string templateSubjectFrom = "";
        private string templateBodyFrom = "";
        public async Task OrderStatusNotify(string email, Order orderId, OrderStatus newOrderStatus)
        {
            var statusIdle = false;
            if (newOrderStatus != orderId.Status) { statusIdle = true; }
            if (statusIdle)
            {
                MemberRenamerDelegate memberRenamer = member => member.Name;
                var templateContentSubject = await File.ReadAllTextAsync(templateSubjectFrom);
                var templateSubject = Template.Parse(templateContentSubject);
                var subject = templateSubject.Render(new { OrderId = orderId }, memberRenamer);
                string from = "Rui Dias";
                var templateContentBody = await File.ReadAllTextAsync(templateBodyFrom);
                var templateBody = Template.Parse(templateContentBody);
                var message = templateBody.Render(
                        new { OrderId = orderId, From = from, NewOrderStatus = newOrderStatus != orderId.Status }
                    );
                await _emailSender.SendEmailAsync(email, subject, message);
            }
        }
    }
}