using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Reference.Commerce.Site.Features.Market.Services;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;
using EPiServer.Reference.Commerce.Site.Features.Product.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared.Extensions;
using EPiServer.Reference.Commerce.Site.Features.Shared.Services;
using EPiServer.Reference.Commerce.Site.Infrastructure.Facades;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B;
using EPiServer.Reference.Commerce.Site.B2B.ServiceContracts;

namespace EPiServer.Reference.Commerce.Site.Features.Product.Controllers
{
    public class ProductController : ContentController<BaseProduct>
    {
        private readonly IPromotionService _promotionService;
        private readonly IContentLoader _contentLoader;
        private readonly IPriceService _priceService;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyservice;
        private readonly IRelationRepository _relationRepository;
        private readonly IQuickOrderService _quickOrderService;
        private readonly UrlResolver _urlResolver;
        private readonly FilterPublished _filterPublished;
        private readonly CultureInfo _preferredCulture;
        private readonly bool _isInEditMode;

        public ProductController(
            IPromotionService promotionService,
            IContentLoader contentLoader,
            IPriceService priceService,
            ICurrentMarket currentMarket,
            CurrencyService currencyservice,
            IRelationRepository relationRepository,
            UrlResolver urlResolver,
            FilterPublished filterPublished,
            PreferredCultureAccessor preferredCultureAccessor,
            IsInEditModeAccessor isInEditModeAccessor,
            IQuickOrderService quickOrderService)
        {
            _promotionService = promotionService;
            _contentLoader = contentLoader;
            _priceService = priceService;
            _currentMarket = currentMarket;
            _currencyservice = currencyservice;
            _relationRepository = relationRepository;
            _urlResolver = urlResolver;
            _preferredCulture = preferredCultureAccessor();
            _isInEditMode = isInEditModeAccessor();
            _filterPublished = filterPublished;
            _quickOrderService = quickOrderService;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(BaseProduct currentContent, string variationCode = "", bool quickview = false)
        {
            var variations = GetVariations(currentContent).ToList();
            if (_isInEditMode && !variations.Any())
            {
                var productWithoutVariation = new BaseProductViewModel
                {
                    Product = currentContent,
                    Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                    CategoryPage = _contentLoader.Get<NodeContent>(_contentLoader.Get<NodeContent>(currentContent.ParentLink).ParentLink),
                    SubcategoryPage = _contentLoader.Get<NodeContent>(currentContent.ParentLink)
                };
                return Request.IsAjaxRequest() ? PartialView("ProductWithoutVariation", productWithoutVariation) : (ActionResult)View("ProductWithoutVariation", productWithoutVariation);
            }

            BaseVariant variation;
            if (!TryGetVariant(variations, variationCode, out variation))
            {
                return HttpNotFound();
            }

            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyservice.GetCurrentCurrency();

            var defaultPrice = GetDefaultPrice(variation, market, currency);
            var discountedPrice = GetDiscountPrice(defaultPrice, market, currency);

            var viewModel = new BaseProductViewModel
            {
                Product = currentContent,
                Variation = variation,
                ListingPrice = defaultPrice != null ? defaultPrice.UnitPrice : new Money(0, currency),
                DiscountedPrice = discountedPrice,
                Colors = variations
                    .Where(x => x.Size != null)
                    .GroupBy(x => x.Color)
                    .Select(g => new SelectListItem
                    {
                        Selected = false,
                        Text = g.First().Color,
                        Value = g.First().Color
                    })
                    .ToList(),
                Sizes = variations
                    .Where(x => x.Color != null && x.Color.Equals(variation.Color, StringComparison.OrdinalIgnoreCase))
                    .Select(x => new SelectListItem
                    {
                        Selected = false,
                        Text = x.Size,
                        Value = x.Size
                    })
                    .ToList(),
                Color = variation.Color,
                Size = variation.Size,
                Images = variation.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                IsAvailable = defaultPrice != null,
                Variants = variations.Select(variant =>
                    {
                        var variantImage = variant.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault();
                        var variantDefaultPrice = GetDefaultPrice(variant, market, currency);
                        return new VariantViewModel
                        {
                            Sku = variant.Code,
                            Size = $"{variant.Color} {variant.Size}",
                            ImageUrl = string.IsNullOrEmpty(variantImage) ? "http://placehold.it/54x54/" : variantImage,
                            DiscountedPrice = GetDiscountPrice(variantDefaultPrice, market, currency),
                            ListingPrice = variantDefaultPrice?.UnitPrice ?? new Money(0, currency),
                            StockQuantity = _quickOrderService.GetTotalInventoryByEntry(variant.Code)
                        };
                    }).ToList(),
                CategoryPage = _contentLoader.Get<NodeContent>(_contentLoader.Get<NodeContent>(currentContent.ParentLink).ParentLink),
                SubcategoryPage = _contentLoader.Get<NodeContent>(currentContent.ParentLink)
            };

            if (Session[Constants.ErrorMesages] != null)
            {
                var messages = Session[Constants.ErrorMesages] as List<string>;
                viewModel.ReturnedMessages = messages;
                Session[Constants.ErrorMesages] = "";
            }

            if (quickview)
            {
                return PartialView("Quickview", viewModel);
            }

            return Request.IsAjaxRequest() ? PartialView(viewModel) : (ActionResult)View(viewModel);
        }

        [HttpPost]
        public ActionResult SelectVariant(BaseProduct currentContent, string color, string size, bool quickview = false)
        {
            var variations = GetVariations(currentContent);

            BaseVariant variation;
            if (TryGetVariantByColorAndSize(variations, color, size, out variation)
                || TryGetVariantByColorAndSize(variations, color, string.Empty, out variation))//if we cannot find variation with exactly both color and size then we will try to get variation by color only
            {
                return RedirectToAction("Index", new { variationCode = variation.Code, quickview });
            }

            return HttpNotFound();
        }

        private IEnumerable<BaseVariant> GetVariations(BaseProduct currentContent)
        {
            return _contentLoader
                .GetItems(currentContent.GetVariants(_relationRepository), _preferredCulture)
                .Cast<BaseVariant>()
                .Where(v => v.IsAvailableInCurrentMarket(_currentMarket) && !_filterPublished.ShouldFilter(v));
        }

        private static bool TryGetVariant(IEnumerable<BaseVariant> variations, string variationCode, out BaseVariant variation)
        {
            variation = !string.IsNullOrEmpty(variationCode) ?
                variations.FirstOrDefault(x => x.Code == variationCode) :
                variations.FirstOrDefault();

            return variation != null;
        }

        private static bool TryGetVariantByColorAndSize(IEnumerable<BaseVariant> variations, string color, string size, out BaseVariant variation)
        {
            variation = variations.FirstOrDefault(x =>
                (string.IsNullOrEmpty(color) || x.Color.Equals(color, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(size) || x.Size.Equals(size, StringComparison.OrdinalIgnoreCase)));

            return variation != null;
        }

        private IPriceValue GetDefaultPrice(BaseVariant variation, IMarket market, Currency currency)
        {
            return _priceService.GetDefaultPrice(
                market.MarketId,
                DateTime.Now,
                new CatalogKey(variation.Code),
                currency);
        }

        private Money? GetDiscountPrice(IPriceValue defaultPrice, IMarket market, Currency currency)
        {
            if (defaultPrice == null)
            {
                return null;
            }

            return _promotionService.GetDiscountPrice(defaultPrice.CatalogKey, market.MarketId, currency).UnitPrice;
        }
    }
}