using System;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Order;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Pricing;

namespace EPiServer.Reference.Commerce.Site.B2B.CustomerProcessors
{
    [ServiceConfiguration(Lifecycle = ServiceInstanceScope.Singleton, ServiceType = typeof(IPlacedPriceProcessor))]
    public class B2BPlacedPriceProcessor : DefaultPlacedPriceProcessor
    {
        private readonly IPriceService _priceService;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        private readonly MapUserKey _mapUserKey;

        public B2BPlacedPriceProcessor(IPriceService priceService, IContentLoader contentLoader,
            ReferenceConverter referenceConverter, MapUserKey mapUserKey, IPriceService priceService1, IContentLoader contentLoader1, ReferenceConverter referenceConverter1, MapUserKey mapUserKey1)
            : base(priceService, contentLoader, referenceConverter, mapUserKey)
        {
            _priceService = priceService1;
            _contentLoader = contentLoader1;
            _referenceConverter = referenceConverter1;
            _mapUserKey = mapUserKey1;
        }

        /// <summary>
        /// Updates the <see cref="T:EPiServer.Commerce.Order.ILineItem"/> item placed price or raises <see cref="T:EPiServer.Commerce.Order.ValidationIssue"/> if their is no valid price.
        /// 
        /// </summary>
        /// <param name="lineItem">The line item.</param><param name="customerContact"/><param name="market">The market.</param><param name="currency">The currency.</param><param name="onValidationError">A callback that is invoked if a validation issue is detected.</param>
        /// <returns>
        /// False if is there is no valid price
        /// </returns>
        public override bool UpdatePlacedPrice(ILineItem lineItem, CustomerContact customerContact, IMarket market, Currency currency, Action<ILineItem, ValidationIssue> onValidationError)
        {
            EntryContentBase entryContent = lineItem.GetEntryContent(_referenceConverter, _contentLoader);
            if (entryContent == null)
            {
                onValidationError(lineItem, ValidationIssue.RemovedDueToUnavailableItem);
                return false;
            }
            if (lineItem.Properties[Constants.Quote.PreQuotePrice] != null && !string.IsNullOrEmpty(lineItem.Properties[Constants.Quote.PreQuotePrice].ToString()))
            {
                return true;
            }

            Money? placedPrice = GetPlacedPrice(entryContent, lineItem.Quantity, customerContact, market, currency);
            if (placedPrice.HasValue)
            {
                if (new Money(currency.Round(lineItem.PlacedPrice), currency) == placedPrice.Value)
                    return true;
                onValidationError(lineItem, ValidationIssue.PlacedPricedChanged);
                lineItem.PlacedPrice = placedPrice.Value.Amount;
                return true;
            }
            onValidationError(lineItem, ValidationIssue.RemovedDueToInvalidPrice);
            return false;
        }
    }
}
