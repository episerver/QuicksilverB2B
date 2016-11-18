using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.Features.AddressBook.Services;
using EPiServer.Reference.Commerce.Site.Features.OrderDetails.Pages;
using EPiServer.Reference.Commerce.Site.Features.OrderDetails.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.OrderHistory.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared.Models;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.Reference.Commerce.Site.Infrastructure.Facades;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Orders;

namespace EPiServer.Reference.Commerce.Site.Features.OrderDetails.Controllers
{
    public class OrderDetailsController : PageController<OrderDetailsPage>
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IOrderRepository _orderRepository;

        public OrderDetailsController(IAddressBookService addressBookService, IOrderRepository orderRepository)
        {
            _addressBookService = addressBookService;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public ActionResult Index(OrderDetailsPage currentPage, int orderGroupId = 0)
        {
            var orderViewModel = new OrderDetailsViewModel
            {
                CurrentPage = currentPage
            };

            var purchaseOrder = _orderRepository.Load<PurchaseOrder>(orderGroupId);
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

            return View(orderViewModel);
        }
    }
}