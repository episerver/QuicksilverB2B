using System.Globalization;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.InventoryService;

namespace EPiServer.Reference.Commerce.Site.B2B.Services
{
    [ServiceConfiguration(typeof(IQuickOrderService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class QuickOrderService : IQuickOrderService
    {
        private readonly IContentLoader _contentLoader;

        public QuickOrderService(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public string ValidateProduct(ContentReference variationReference, decimal quantity, string code)
        {
            if (ContentReference.IsNullOrEmpty(variationReference))
            {
                return $"The product with SKU {code} does not exist.";
            }

            var variantContent = _contentLoader.Get<VariationContent>(variationReference);
            var maxQuantity = GetTotalInventoryByEntry(variantContent.Code);
            if (quantity > maxQuantity)
            {
                return $"Quantity ordered is bigger than in stock quantity for the product with SKU {code}.";
            }
            return null;
        }

        public ProductViewModel GetProductByCode(ContentReference productReference)
        {
            var product = new ProductViewModel();
            if (!ContentReference.IsNullOrEmpty(productReference))
            {
                var variantContent = _contentLoader.Get<VariationContent>(productReference);
                product.ProductName = variantContent.Name;
                product.Sku = variantContent.Code;
                product.UnitPrice = (variantContent.GetDefaultPrice() != null ) ? variantContent.GetDefaultPrice().UnitPrice.Amount : 0;
            }
            return product;
        }

        public decimal GetTotalInventoryByEntry(string code)
        {
            var inventoryService = ServiceLocator.Current.GetInstance<IInventoryService>();
            return inventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);
        }
    }
}
