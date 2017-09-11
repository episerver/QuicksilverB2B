using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Find.Commerce;
using EPiServer.Reference.Commerce.Site.Features.Search.Models;
using EPiServer.Reference.Commerce.Site.Features.Shared.Services;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Pricing;

namespace EPiServer.Reference.Commerce.Site.Features.Shared.Extensions
{
    public static class ProductContentExtensions
    {
#pragma warning disable 649
        private static Injected<IRelationRepository> _relationRepository;
        private static Injected<IContentLoader> _contentLoader;
        private static Injected<IPriceService> _priceService;
        private static Injected<IPromotionService> _promotionService;
#pragma warning restore 649

        public static string ParentName(this ProductContent productContent)
        {
            var parent = _contentLoader.Service.Get<NodeContent>(productContent.ParentLink);
            return parent?.DisplayName;
        }

        public static string TopCategory(this ProductContent productContent)
        {
            var parent = _contentLoader.Service.Get<NodeContent>(productContent.ParentLink);
            if (parent == null) return null;

            var topParent = _contentLoader.Service.Get<NodeContent>(parent.ParentLink);
            return topParent?.DisplayName;
        }

        public static List<Price> OriginalPrices(this ProductContent productContent)
        {
            return productContent.OriginalPriceValues().Select(p => new Price(p)).ToList();
        }

        public static List<Price> ListingPrices(this ProductContent productContent)
        {
            var originalPrices = productContent.OriginalPriceValues();
            var discountedPrices = new List<Price>();
            foreach (var originalPrice in originalPrices)
            {
                var discountedPrice = _promotionService.Service.GetDiscountPrice(originalPrice.CatalogKey,
                    originalPrice.MarketId, originalPrice.UnitPrice.Currency);
                discountedPrices.Add(new Price(discountedPrice));
            }
            return
                discountedPrices;
        }

        private static List<IPriceValue> OriginalPriceValues(this ProductContent productContent)
        {
            var validPrices =
                productContent.Prices()
                    .Where(x => x.ValidFrom <= DateTime.Now && (x.ValidUntil == null || x.ValidUntil >= DateTime.Now));

            var originalPrices = new List<IPriceValue>();
            foreach (var marketPrices in validPrices.GroupBy(x => x.MarketId))
            {
                foreach (var currencyPrices in marketPrices.GroupBy(x => x.UnitPrice.Currency))
                {
                    var topPrice = currencyPrices.OrderByDescending(x => x.UnitPrice).FirstOrDefault();
                    if (topPrice == null)
                        continue;

                    originalPrices.Add(topPrice);
                }
            }

            return originalPrices;
        }

        public static IEnumerable<IPriceValue> Prices(this ProductContent productContent)
        {
            return Prices(productContent, _priceService.Service);
        }

        public static IEnumerable<IPriceValue> Prices(this ProductContent productContent,
            IPriceService priceService)
        {

            List<VariationContent> variants = productContent.VariationContents().ToList();
            return
                priceService.GetCatalogEntryPrices(
                    variants.Select(x => new CatalogKey(x.Code))).ToList();
        }

        public static IEnumerable<VariationContent> VariationContents(this ProductContent productContent)
        {
            return VariationContents(productContent, _contentLoader.Service, _relationRepository.Service);
        }

        public static IEnumerable<VariationContent> VariationContents(this ProductContent productContent,
            IContentLoader contentLoader, IRelationRepository relationRepository)
        {
            return
                contentLoader.GetItems(productContent.GetVariants(relationRepository), productContent.Language)
                    .OfType<VariationContent>();
        }

        public static IEnumerable<string> GetParentBlackListACL(this ProductContent productContent)
        {
            var parents = productContent.ParentNodeRelations();
            if (parents == null || !parents.Any()) return Enumerable.Empty<string>();

            return parents.SelectMany(p =>
            {
                var parent = _contentLoader.Service.Get<BaseNode>(p);
                if (parent == null) return Enumerable.Empty<string>();

                return parent.ACLBlackList?.Split(',') ?? Enumerable.Empty<string>();
            });
        }
    }
}