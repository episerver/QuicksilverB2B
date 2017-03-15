using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Commerce.Order;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.AddressBook.Services;
using EPiServer.Reference.Commerce.Site.Features.OrderDetails.Pages;
using EPiServer.Reference.Commerce.Site.Features.OrderDetails.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared.Models;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Cart.ViewModelFactories;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Orders;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.Core;

namespace EPiServer.Reference.Commerce.Site.Features.OrderDetails.Controllers
{
    public class OrderDetailsController : PageController<OrderDetailsPage>
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IOrdersService _ordersService;
        private readonly ICustomerService _customerService;
        private readonly IOrderRepository _orderRepository;
        private readonly IContentLoader _contentLoader;
        private readonly ICartService _cartService;

        public OrderDetailsController(IAddressBookService addressBookService, IOrdersService ordersService, ICustomerService customerService, IOrderRepository orderRepository, IContentLoader contentLoader, ICartService cartService)
        {
            _addressBookService = addressBookService;
            _ordersService = ordersService;
            _customerService = customerService;
            _orderRepository = orderRepository;
            _contentLoader = contentLoader;
            _cartService = cartService;
        }

        [HttpGet]
        public ActionResult Index(OrderDetailsPage currentPage, int orderGroupId = 0)
        {
            var orderViewModel = new OrderDetailsViewModel
            {
                CurrentPage = currentPage,
                CurrentCustomer = _customerService.GetCurrentContact()
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
            if (purchaseOrder[Constants.Quote.QuoteExpireDate] != null &&
                !string.IsNullOrEmpty(purchaseOrder[Constants.Quote.QuoteExpireDate].ToString()))
            {
                DateTime quoteExpireDate;
                DateTime.TryParse(purchaseOrder[Constants.Quote.QuoteExpireDate].ToString(), out quoteExpireDate);
                if (DateTime.Compare(DateTime.Now, quoteExpireDate) > 0)
                {
                    orderViewModel.QuoteStatus = Constants.Quote.QuoteExpired;
                    try
                    {
                        // Update order quote status to expired
                        purchaseOrder[Constants.Quote.QuoteStatus] = Constants.Quote.QuoteExpired;
                        _orderRepository.Save(purchaseOrder);
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(GetType()).Error("Failed to update order status to Quote Expired.", ex.StackTrace);
                    }

                }
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

        [HttpPost]
        public ActionResult ApproveOrder(int orderGroupId = 0)
        {
            if (orderGroupId == 0)
                return Json(new { result = true});
            var success =_ordersService.ApproveOrder(orderGroupId);

            return success ? Json(new { result = true }) : Json(new {result = "Failed to process your payment."});
        }

        [HttpPost]
        public ActionResult Reorder(int orderGroupId = 0)
        {
            var purchaseOrder = OrderContext.Current.GetPurchaseOrderById(orderGroupId);
            var form = purchaseOrder.GetFirstForm();
            var list = form.GetAllLineItems();

            string warningMessage = string.Empty;
            
            ICart Cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName);

            foreach (var item in list)
            {
                Cart.AddLineItem(item);
               
            }

           var order = _orderRepository.Save(Cart);
            return Json(new { result = true });
        }

    }
}