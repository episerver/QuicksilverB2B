using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Orders.Pages;
using EPiServer.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Orders.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.Orders
{ 
    [Authorize]
    public class OrdersPageController : PageController<OrdersPage>
    {
        public ActionResult Index(OrdersPage currentPage)
        {
            var viewModel = new OrdersPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
        public ActionResult Edit(OrdersPage currentPage)
        {
            var viewModel = new OrdersPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
        public ActionResult AddSub(OrdersPage currentPage)
        {
            var viewModel = new OrdersPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
    }
}