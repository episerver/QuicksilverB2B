using System.Collections.Generic;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Orders.Pages;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Orders.Controllers
{
    [Authorize]
    public class OrdersPageController : PageController<OrdersPage>
    {
        private readonly ICustomerService _customerService;
        private readonly IOrdersService _ordersService;

        public OrdersPageController(ICustomerService customerService, IOrdersService ordersService)
        {
            _customerService = customerService;
            _ordersService = ordersService;
        }

        public ActionResult Index(OrdersPage currentPage)
        {
            var organizationUsersList = _customerService.GetContactsForCurrentOrganization();
            var viewModel = new OrdersPageViewModel
            {
                CurrentPage = currentPage
            };

            var ordersOrganization = new List<OrderOrganizationViewModel>();
            foreach (var user in organizationUsersList)
            {
               _ordersService.GetUserOrders(user.ContactId, out ordersOrganization);
            }
            viewModel.OrdersOrganization = ordersOrganization;
            
            return View(viewModel);
        }

        public ActionResult QuickOrder(OrdersPage currentPage)
        {
            var viewModel = new OrdersPageViewModel { CurrentPage = currentPage};
            return View(viewModel);
        }
    }
}