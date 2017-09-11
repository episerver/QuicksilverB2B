using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;
using EPiServer.Reference.Commerce.Site.Features.Shared.Extensions;
using EPiServer.Reference.Commerce.Site.Features.Shared.Services;
using EPiServer.Reference.Commerce.Site.Infrastructure.Facades;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using EPiServer.Reference.Commerce.Site.Features.Market.Services;
using EPiServer.Reference.Commerce.Site.Features.Product.ViewModels;
using Mediachase.Commerce.Inventory;
using System;

namespace EPiServer.Reference.Commerce.Site.Features.Product.Services
{
    [ServiceConfiguration(typeof(IProductService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class ProductService : IProductService
    {
        private readonly IContentLoader _contentLoader;
        private readonly IPromotionService _promotionService;
        private readonly IPricingService _pricingService;
        private readonly UrlResolver _urlResolver;
        private readonly IRelationRepository _relationRepository;
        private readonly CultureInfo _preferredCulture;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly ReferenceConverter _referenceConverter;

        public ProductService(IContentLoader contentLoader,
            IPromotionService promotionService,
            IPricingService pricingService,
            UrlResolver urlResolver,
            IRelationRepository relationRepository,
            ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            ReferenceConverter referenceConverter)
        {
            _contentLoader = contentLoader;
            _promotionService = promotionService;
            _pricingService = pricingService;
            _urlResolver = urlResolver;
            _relationRepository = relationRepository;
            _preferredCulture = ContentLanguage.PreferredCulture;
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _referenceConverter = referenceConverter;
        }

        public IEnumerable<BaseVariant> GetVariations(BaseProduct currentContent)
        {
            return _contentLoader
                .GetItems(currentContent.GetVariants(_relationRepository), _preferredCulture)
                .Cast<BaseVariant>()
                .Where(v => v.IsAvailableInCurrentMarket(_currentMarket));
        }

        public string GetSiblingVariantCodeBySize(string siblingCode, string size)
        {
            ContentReference variationReference = _referenceConverter.GetContentLink(siblingCode);
            IEnumerable<ProductVariation> productRelations = _relationRepository.GetParents<ProductVariation>(variationReference).ToList();
            IEnumerable<ProductVariation> siblingsRelations = _relationRepository.GetChildren<ProductVariation>(productRelations.First().Parent);
            IEnumerable<ContentReference> siblingsReferences = siblingsRelations.Select(x => x.Child);
            IEnumerable<IContent> siblingVariations = _contentLoader.GetItems(siblingsReferences, _preferredCulture).ToList();

            var siblingVariant = siblingVariations.OfType<BaseVariant>().First(x => x.Code == siblingCode);

            foreach (var variant in siblingVariations.OfType<BaseVariant>())
            {
                if (variant.Size.Equals(size, StringComparison.OrdinalIgnoreCase) && variant.Code != siblingCode
                    && variant.Color.Equals(siblingVariant.Color, StringComparison.OrdinalIgnoreCase))
                {
                    return variant.Code;
                }
            }

            return null;
        }

        public IEnumerable<ProductViewModel> GetVariationsAndPricesForProducts(IEnumerable<ProductContent> products)
        {
            var variationsToLoad = new Dictionary<ContentReference, ContentReference>();
            var fashionProducts = products.ToList();
            foreach (var product in fashionProducts)
            {
                var relations = _relationRepository.GetChildren<ProductVariation>(product.ContentLink);
                variationsToLoad.Add(relations.First().Child, product.ContentLink);
            }

            var variations = _contentLoader.GetItems(variationsToLoad.Select(x => x.Key), _preferredCulture).Cast<BaseVariant>();

            var productModels = new List<ProductViewModel>();

            foreach (var variation in variations)
            {
                var productContentReference = variationsToLoad.First(x => x.Key == variation.ContentLink).Value;
                var product = fashionProducts.First(x => x.ContentLink == productContentReference);
                productModels.Add(CreateProductViewModel(product, variation));
            }
            return productModels;
        }

        public virtual ProductViewModel GetProductViewModel(ProductContent product)
        {
            var variations = _contentLoader.GetItems(product.GetVariants(), _preferredCulture).
                                            Cast<VariationContent>()
                                           .ToList();

            var variation = variations.FirstOrDefault();
            return CreateProductViewModel(product, variation);
        }

        public virtual ProductViewModel GetProductViewModel(VariationContent variation)
        {
            return CreateProductViewModel(null, variation);
        }

        private ProductViewModel CreateProductViewModel(ProductContent product, VariationContent variation)
        {
            if (variation == null)
            {
                return null;
            }

            ContentReference productContentReference;
            if (product != null)
            {
                productContentReference = product.ContentLink;
            }
            else
            {
                productContentReference = variation.GetParentProducts(_relationRepository).FirstOrDefault();
                if (ContentReference.IsNullOrEmpty(productContentReference))
                {
                    return null;
                }
            }
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();

            var originalPrice = _pricingService.GetCurrentPrice(variation.Code);
            var discountedPrice = originalPrice.HasValue ? GetDiscountPrice(variation, market, currency, originalPrice.Value) : (Money?)null;

            var image = variation.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault() ?? "";
            var brand = product is BaseProduct ? ((BaseProduct)product).Brand : string.Empty;

            return new ProductViewModel
            {
                DisplayName = product != null ? product.DisplayName : variation.DisplayName,
                PlacedPrice = originalPrice.HasValue ? originalPrice.Value : new Money(0, currency),
                DiscountedPrice = discountedPrice,
                ImageUrl = image,
                Url = variation.GetUrl(),
                Brand = brand,
                IsAvailable = originalPrice.HasValue
            };
        }

        private Money GetDiscountPrice(VariationContent variation, IMarket market, Currency currency, Money originalPrice)
        {
            var discountedPrice = _promotionService.GetDiscountPrice(new CatalogKey(variation.Code), market.MarketId, currency);
            if (discountedPrice != null)
            {
                return discountedPrice.UnitPrice;
            }

            return originalPrice;
        }
    }
}