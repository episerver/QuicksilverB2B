using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.AddressBook.Services;
using EPiServer.Reference.Commerce.Site.Features.OrderDetails.Pages;
using EPiServer.Reference.Commerce.Site.Features.OrderDetails.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared.Models;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Orders;

namespace EPiServer.Reference.Commerce.Site.Features.OrderDetails.Controllers
{
    public class OrderDetailsController : PageController<OrderDetailsPage>
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IOrdersService _ordersService;

        public OrderDetailsController(IAddressBookService addressBookService, IOrdersService ordersService)
        {
            _addressBookService = addressBookService;
            _ordersService = ordersService;
        }

        [HttpGet]
        public ActionResult Index(OrderDetailsPage currentPage, int orderGroupId = 0)
        {
            var orderViewModel = new OrderDetailsViewModel
            {
                CurrentPage = currentPage
            };

            var purchaseOrder = OrderContext.Current.GetPurchaseOrderById(orderGroupId);
            if (purchaseOrder == null) return View(orderViewModel);

            // Assume there is only one form per purchase.
            var form = purchaseOrder.GetFirstForm();

            var billingAddress = form.Payments.FirstOrDefault() != null
                ? form.Payments.First().BillingAddress
                : new OrderAddress();

            orderViewModel.PurchaseOrder = purchaseOrder;
            orderViewModel.Items = form.GetAllLineItems().Select(lineItem => new OrderDetailsItemViewModel
            {
                LineItem = lineItem,
            }).GroupBy(x => x.LineItem.Code).Select(group => @group.First());
            orderViewModel.BillingAddress = _addressBookService.ConvertToModel(billingAddress);
            orderViewModel.ShippingAddresses = new List<AddressModel>();

            foreach (var orderAddress in form.Shipments.Select(s => s.ShippingAddress))
            {
                var shippingAddress = _addressBookService.ConvertToModel(orderAddress);
                orderViewModel.ShippingAddresses.Add(shippingAddress);
                orderViewModel.OrderGroupId = purchaseOrder.OrderGroupId;
            }

            if (!string.IsNullOrEmpty(purchaseOrder["QuoteStatus"]?.ToString()) &&
                (purchaseOrder.Status == OrderStatus.InProgress.ToString() ||
                 purchaseOrder.Status == OrderStatus.OnHold.ToString()))
            {
                orderViewModel.QuoteStatus = purchaseOrder["QuoteStatus"].ToString();
            }

            orderViewModel.BudgetPayment = _ordersService.GetOrderBudgetPayment(purchaseOrder);

            return View(orderViewModel);
        }

        [HttpGet]
        public ActionResult ApproveOrder(OrderDetailsPage currentPage, int orderGroupId = 0)
        {
            if (orderGroupId == 0) RedirectToAction("Index", new { node = currentPage.ContentLink, orderGroupId = orderGroupId });

            _ordersService.ApproveOrder(orderGroupId);
            return RedirectToAction("Index", new { node = currentPage.ContentLink, orderGroupId = orderGroupId });
        }
    }
}