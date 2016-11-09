using System;
using EPiServer.Logging;
using Mediachase.BusinessFoundation;
using Mediachase.Commerce.Manager.Order.CommandHandlers.PurchaseOrderHandlers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;

namespace EPiServer.Reference.Commerce.Manager.CustomCommands
{
    public class FinishQuoteRequest : TransactionCommandHandler
    {
        protected override bool IsCommandEnable(OrderGroup order, CommandParameters cp)
        {
            bool flag = base.IsCommandEnable(order, cp);
            if (flag && !string.IsNullOrEmpty(order["QuoteStatus"] as string) )
                flag = order["QuoteStatus"].ToString() == "RequestQuotation";
            return flag;
        }

        protected override void DoCommand(OrderGroup order, CommandParameters cp)
        {
            try
            {
                PurchaseOrder purchaseOrder = order as PurchaseOrder;
                purchaseOrder["QuoteExpireDate"] = DateTime.Now.AddDays(30);
                purchaseOrder["QuoteStatus"] = "RequestQuotationFinished";
                OrderStatusManager.ReleaseHoldOnOrder(purchaseOrder);
                AddNoteToPurchaseOrder("OrderNote_ChangeOrderStatusPattern", purchaseOrder, purchaseOrder.Status);
                SavePurchaseOrderChanges(purchaseOrder);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error("Failed to process request quotation approve.", ex);
            }
        }
    }
}