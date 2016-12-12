using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.Entities;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.Reference.Commerce.Site.Features.Cart.Services;
using EPiServer.Reference.Commerce.Site.Features.Folder.Pages;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.Blocks;
using EPiServer.Reference.Commerce.Site.Features.QuickOrder.Pages;
using EPiServer.Reference.Commerce.Site.Features.Search.Services;
using EPiServer.Reference.Commerce.Site.Infrastructure.Attributes;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Catalog;
using EPiServer.Find.Helpers.Text;


namespace EPiServer.Reference.Commerce.Site.Features.QuickOrder.Controllers
{
    public class QuickOrderBlockController : BlockController<QuickOrderBlock>
    {
        private readonly IQuickOrderService _quickOrderService;
        private readonly ICartService _cartService;
        private readonly IFileHelperService _fileHelperService;
        private ICart _cart;
        private readonly IOrderRepository _orderRepository;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IEPiFindSearchService _ePiFindSearchService;

        public QuickOrderBlockController(
            IQuickOrderService quickOrderService,
            ICartService cartService,
            IFileHelperService fileHelperService,
            IOrderRepository orderRepository,
            ReferenceConverter referenceConverter,
            IEPiFindSearchService ePiFindSearchService)
        {
            _quickOrderService = quickOrderService;
            _cartService = cartService;
            _fileHelperService = fileHelperService;
            _orderRepository = orderRepository;
            _referenceConverter = referenceConverter;
            _ePiFindSearchService = ePiFindSearchService;
        }
        public override ActionResult Index(QuickOrderBlock currentBlock)
        {
            currentBlock.ReturnedMessages = TempData["messages"] as List<string>;
            currentBlock.ProductsList = TempData["products"] as List<ProductViewModel>;
            return PartialView(currentBlock);
        }

        [HttpPost]
        [AllowDBWrite]
        public ActionResult Import(ProductViewModel[] ProductsList)
        {
            var returnedMessages = new List<string>();

            ModelState.Clear();

            if (Cart == null)
            {
                _cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName);
            }

            foreach (var product in ProductsList)
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

            var quickOrderPage = GetQuickOrderPage();
            return Redirect(quickOrderPage?.LinkURL ?? Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult AddFromFile()
        {
            var quickOrderPage = GetQuickOrderPage();

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

                var fileData = _fileHelperService.GetImportData<QuickOrderData>(uploadedFile);
                foreach (var record in fileData)
                {
                    //find the product
                    ContentReference variationReference = _referenceConverter.GetContentLink(record.Sku);
                    var product = _quickOrderService.GetProductByCode(variationReference);

                    product.Quantity = record.Quantity;
                    product.TotalPrice = product.Quantity * product.UnitPrice;

                    productsList.Add(product);
                }
                TempData["products"] = productsList.Count > 0 ? productsList : null;
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
            var data = _ePiFindSearchService.SearchSkus(query);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private ICart Cart
        {
            get { return _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName)); }
        }

        private QuickOrderPage GetQuickOrderPage()
        {
            var homePage = DataFactory.Instance.GetPage(ContentReference.StartPage);
            var folderPage = DataFactory.Instance.GetChildren<FolderPage>(homePage.PageLink).FirstOrDefault();
            return DataFactory.Instance.GetChildren<QuickOrderPage>(folderPage?.PageLink).FirstOrDefault();
        }
    }
}