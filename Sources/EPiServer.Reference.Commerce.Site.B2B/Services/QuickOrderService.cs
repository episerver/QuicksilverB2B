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
                return $"The max quantity for the product with SKU {code} is {maxQuantity}.";
            }
            return null;
        }

        public ProductViewModel GetProductByCode(ContentReference productReference)
        {
            var product = new ProductViewModel();
            if (!ContentReference.IsNullOrEmpty(productReference))
            {
                var variantContent = _contentLoader.Get<VariationContent>(productReference);
                float unitPrice;
                float.TryParse(variantContent.GetDefaultPrice().ToPriceValue().UnitPrice.Amount.ToString(CultureInfo.InvariantCulture), out unitPrice);
                product.ProductName = variantContent.Name;
                product.Sku = variantContent.Code;
                product.UnitPrice = unitPrice;
            }
            return product;
        }

        private decimal GetTotalInventoryByEntry(string code)
        {
            var inventoryService = ServiceLocator.Current.GetInstance<IInventoryService>();
            return inventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);
        }
    }
}
