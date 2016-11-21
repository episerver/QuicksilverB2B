using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Folder.Pages;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.Pages;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Catalog;

namespace EPiServer.Reference.Commerce.Site.Features.QuickOrder.Controllers
{
    [Authorize]
    public class QuickOrderPageController : PageController<QuickOrderPage>
    {
        private readonly IQuickOrderService _quickOrderService;
        private readonly ICartService _cartService;
        private ICart _cart;
        private readonly IOrderRepository _orderRepository;
        private readonly ReferenceConverter _referenceConverter;

        public QuickOrderPageController(
            IQuickOrderService quickOrderService,
            ICartService cartService,
            IOrderRepository orderRepository,
            ReferenceConverter referenceConverter)
        {
            _quickOrderService = quickOrderService;
            _cartService = cartService;
            _orderRepository = orderRepository;
            _referenceConverter = referenceConverter;
        }

        public ActionResult Index(QuickOrderPage currentPage)
        {
            var messages = TempData["messages"] as List<string>;
            var products = TempData["products"] as List<ProductViewModel>;
            var viewModel = new QuickOrderPageViewModel
            {
                CurrentPage = currentPage,
                ReturnedMessages = messages,
                ProductsList = products
            };
            return View(viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult Import(QuickOrderPageViewModel viewModel)
        {
            var returnedMessages = new List<string>();

            ModelState.Clear();

            if (Cart == null)
            {
                _cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName);
            }

            foreach (var product in viewModel.ProductsList)
            {
                if (!product.ProductName.Equals("removed"))
                {
                    ContentReference variationReference = _referenceConverter.GetContentLink(product.Sku);
                    var responseMessage = _quickOrderService.ValidateProduct(variationReference, Convert.ToDecimal(product.Quantity), product.Sku);
                    if (responseMessage.IsNullOrEmpty())
                    {
                        string warningMessage;
                        if (_cartService.AddToCart(Cart, product.Sku, out warningMessage))
                        {
                            _cartService.ChangeCartItem(Cart, 0, product.Sku, product.Quantity, "", "");
                            _orderRepository.Save(Cart);
                        }
                    }
                    else
                    {
                        returnedMessages.Add(responseMessage);
                    }
                }
            }
            if (returnedMessages.Count == 0)
            {
                returnedMessages.Add("All items were added to cart.");
            }
            TempData["messages"] = returnedMessages;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddFromFile()
        {
            // Get URL of the quick order page
            var homePage = DataFactory.Instance.GetPage(ContentReference.StartPage);
            var folderPage = DataFactory.Instance.GetChildren<FolderPage>(homePage.PageLink).FirstOrDefault();
            var quickOrderPage = DataFactory.Instance.GetChildren<QuickOrderPage>(folderPage?.PageLink).FirstOrDefault();

            HttpPostedFileBase fileContent = Request.Files[0];
            if (fileContent != null && fileContent.ContentLength > 0)
            {
                Stream uploadedFile = fileContent.InputStream;
                var fileName = fileContent.FileName;
                var productsList = new List<ProductViewModel>();

                //validation for csv
                if (!fileName.Contains(".csv"))
                {
                    TempData["messages"] = new List<string>() { "The uploaded file is not valid!" };
                    return Json(new { data = quickOrderPage?.LinkURL });
                }

                var reader = new StreamReader(uploadedFile);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line != null)
                    {
                        line = line.Replace("\"", "");

                        var values = line.Split(',');
                        var sku = values[0];
                        var fileQuantity = values[1];

                        //find the product
                        ContentReference variationReference = _referenceConverter.GetContentLink(sku);
                        var product = _quickOrderService.GetProductByCode(variationReference);

                        int quantity;
                        if (Int32.TryParse(fileQuantity, out quantity))
                        {
                            product.Quantity = quantity;
                            product.TotalPrice = product.Quantity * product.UnitPrice;
                        }
                        productsList.Add(product);
                    }
                }
                TempData["products"] = productsList;
            }
            else
            {
                TempData["messages"] = new List<string>() { "The uploaded file is not valid!" };
                return Json(new { data = quickOrderPage?.LinkURL });
            }
            return Json(new { data = quickOrderPage?.LinkURL });
        }

        public JsonResult GetSku(string query)
        {
            var data = new[] {
              new { sku = "1111", productName = "product1", unitPrice = "5"},
              new { sku = "222", productName = "product2", unitPrice = "15"},
              new { sku = "312", productName = "product3", unitPrice = "25"}
           };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private ICart Cart
        {
            get { return _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName)); }
        }
    }
}