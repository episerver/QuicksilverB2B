using System;
using EPiServer.Reference.Commerce.Site.Features.AddressBook.Services;
using EPiServer.Reference.Commerce.Site.Features.OrderHistory.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.Models;
using EPiServer.Reference.Commerce.Site.Infrastructure.Facades;
using EPiServer.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.Features.OrderHistory.ViewModels;
using Mediachase.Commerce.Orders;
using EPiServer.Reference.Commerce.Site.B2B;

namespace EPiServer.Reference.Commerce.Site.Features.OrderHistory.Controllers
{
    [Authorize]
    public class OrderHistoryController : PageController<OrderHistoryPage>
    {
        private readonly CustomerContextFacade _customerContext;
        private readonly IAddressBookService _addressBookService;
        private readonly IOrderRepository _orderRepository;

        public OrderHistoryController(CustomerContextFacade customerContextFacade, IAddressBookService addressBookService, IOrderRepository orderRepository)
        {
            _customerContext = customerContextFacade;
            _addressBookService = addressBookService;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public ActionResult Index(OrderHistoryPage currentPage)
        {
            var iPurchaseOrders = _orderRepository.Load<IPurchaseOrder>(_customerContext.CurrentContactId)
                                             .OrderByDescending(x => x.Created)
                                             .ToList();

            var purchaseOrders = iPurchaseOrders.Cast<PurchaseOrder>().ToList();

            var viewModel = new OrderHistoryViewModel
            {
                CurrentPage = currentPage,
                Orders = new List<OrderViewModel>()
            };

            foreach (var purchaseOrder in purchaseOrders)
            {
                // Assume there is only one form per purchase.
                var form = purchaseOrder.GetFirstForm();
 
                var billingAddress = form.Payments.FirstOrDefault() != null ? form.Payments.First().BillingAddress : new OrderAddress();
                var orderViewModel = new OrderViewModel
                {
                    PurchaseOrder = purchaseOrder,
                    Items = form.GetAllLineItems().Select(lineItem => new OrderHistoryItemViewModel
                    {
                        LineItem = lineItem,
                    }).GroupBy(x => x.LineItem.Code).Select(group => group.First()),
                    BillingAddress = _addressBookService.ConvertToModel(billingAddress),
                    ShippingAddresses = new List<AddressModel>()
                };

                foreach (var orderAddress in form.Shipments.Select(s => s.ShippingAddress))
                {
                    var shippingAddress = _addressBookService.ConvertToModel(orderAddress);
                    orderViewModel.ShippingAddresses.Add(shippingAddress);
                    orderViewModel.OrderGroupId = purchaseOrder.OrderGroupId;
                }

                if (!string.IsNullOrEmpty(purchaseOrder[Constants.Quote.QuoteStatus]?.ToString()) && 
                    (purchaseOrder.Status == OrderStatus.InProgress.ToString() || purchaseOrder.Status == OrderStatus.OnHold.ToString()) )
                {
                    orderViewModel.QuoteStatus = purchaseOrder[Constants.Quote.QuoteStatus].ToString();
                    DateTime quoteExpireDate;
                    DateTime.TryParse(purchaseOrder[Constants.Quote.QuoteExpireDate].ToString(), out quoteExpireDate);
                    if (DateTime.Compare(DateTime.Now, quoteExpireDate) > 0)
                    {
                        orderViewModel.QuoteStatus = Constants.Quote.QuoteExpired;
                    }
                }

                viewModel.Orders.Add(orderViewModel);
            }

            return View(viewModel);
        }
    }
}