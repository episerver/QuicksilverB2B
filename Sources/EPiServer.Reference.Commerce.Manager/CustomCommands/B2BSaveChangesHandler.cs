using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.B2B;
using Mediachase.BusinessFoundation;
using Mediachase.Commerce;
using Mediachase.Commerce.Engine;
using Mediachase.Commerce.Manager.Apps_Code.Order;
using Mediachase.Commerce.Manager.Order.CommandHandlers.PurchaseOrderHandlers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.MetaDataPlus;
using Mediachase.Web.Console.BaseClasses;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.Reference.Commerce.Manager.CustomCommands
{
    public class B2BSaveChangesHandler : SaveChangesHandler
    {
        protected override void DoCommand(IOrderGroup order, CommandParameters cp)
        {
            Mediachase.Ibn.Web.UI.CHelper.RequireDataBind();
            PurchaseOrder purchaseOrder = order as PurchaseOrder;
            WorkflowResults workflowResults = OrderGroupWorkflowManager.RunWorkflow(purchaseOrder, "SaveChangesWorkflow",
                false,
                false,
                new Dictionary<string, object>
                {
                    {
                        "PreventProcessPayment",
                        !string.IsNullOrEmpty(order.Properties["QuoteStatus"] as string) &&
                        (order.Properties["QuoteStatus"].ToString() == Constants.Quote.RequestQuotation||
                        order.Properties["QuoteStatus"].ToString() == Constants.Quote.RequestQuotationFinished)
                    }
                });

            if (workflowResults.Status != WorkflowStatus.Completed)
            {
                string msg = "Unknow error";
                if (workflowResults.Exception != null)
                    msg = workflowResults.Exception.Message;
                ErrorManager.GenerateError(msg);
            }
            else
            {
                WritePurchaseOrderChangeNotes(purchaseOrder);
                SavePurchaseOrderChanges(purchaseOrder);
                PurchaseOrderManager.UpdatePromotionUsage(purchaseOrder, purchaseOrder);
                OrderHelper.ExitPurchaseOrderFromEditMode(purchaseOrder.OrderGroupId);
            }
        }

        private static void WritePurchaseOrderChangeNotes(PurchaseOrder purchaseOrder)
        {
            foreach (Shipment shipment in purchaseOrder.OrderForms.ToArray().SelectMany(x => x.Shipments.ToArray()))
            {
                WriteChangedLineItemNotes(purchaseOrder, shipment);
                WriteMovedLineItemNotes(purchaseOrder, shipment);
                WriteChangedPaymentsNotes(purchaseOrder);
            }
            WriteReturnedLineItemNotes(purchaseOrder);
        }
        private static void WriteChangedLineItemNotes(PurchaseOrder purchaseOrder, Shipment shipment)
        {
            foreach (LineItem lineItem in Shipment.GetClonedLineItemsForShipment(shipment).Concat(shipment.Parent.LineItems.DeletedLineItems))
            {
                string noteDetaisPatternName = null;
                decimal num = lineItem.Quantity;
                switch (lineItem.ObjectState)
                {
                    case MetaObjectState.Added:
                        noteDetaisPatternName = "OrderNote_AddNewLineItemPattern";
                        break;
                    case MetaObjectState.Deleted:
                        noteDetaisPatternName = "OrderNote_DeleteLineItemPattern";
                        break;
                    default:
                        if (lineItem.ObjectState == MetaObjectState.Modified && lineItem.OldQuantity != decimal.Zero)
                        {
                            noteDetaisPatternName = "OrderNote_ChangeLineItemPattern";
                            num = lineItem.Quantity - lineItem.OldQuantity;
                        }
                        break;
                }
                if (noteDetaisPatternName == null || num == decimal.Zero) continue;

                string str = new Money(purchaseOrder.Total, new Currency(purchaseOrder.BillingCurrency)).ToString();
                AddNoteToOrder(purchaseOrder, noteDetaisPatternName, num.ToString("F2"), lineItem.DisplayName,
                    shipment.ShipmentId.ToString(), str);
            }
        }

        private static void WriteMovedLineItemNotes(PurchaseOrder purchaseOrder, Shipment shipment)
        {
            if (shipment.ObjectState != MetaObjectState.Added)
                return;
            string noteDetaisPatternName = "OrderNote_MoveLineItemPattern";
            foreach (LineItem lineItem in Shipment.GetClonedLineItemsForShipment(shipment))
            {
                string str = Shipment.GetLineItemQuantity(shipment, lineItem.LineItemId).ToString("F2");
                AddNoteToOrder(purchaseOrder, noteDetaisPatternName, str, lineItem.DisplayName, shipment.ShipmentId.ToString());
            }
        }

        private static void WriteReturnedLineItemNotes(PurchaseOrder purchaseOrder)
        {
            foreach (OrderForm orderForm in purchaseOrder.ReturnOrderForms)
            {
                if (orderForm.ObjectState == MetaObjectState.Added)
                {
                    foreach (LineItem lineItem in orderForm.LineItems)
                    {
                        string noteDetaisPatternName = "OrderNote_ReturnLineItemPattern";
                        string str = lineItem.Quantity.ToString("F2");
                        PurchaseOrder purchaseOrder1 = purchaseOrder;
                        string[] strArray = {
                            str,
                            lineItem.DisplayName,
                            orderForm.RMANumber,
                            lineItem.ReturnReason
                        };
                        AddNoteToOrder(purchaseOrder1, noteDetaisPatternName, strArray);
                    }
                }
            }
        }

        private static void WriteChangedPaymentsNotes(PurchaseOrder purchaseOrder)
        {
            foreach (Payment payment in purchaseOrder.OrderForms.ToArray().SelectMany(x => x.Payments.ToArray()))
            {
                string noteDetaisPatternName = null;
                if (payment.ObjectState == MetaObjectState.Added)
                    noteDetaisPatternName = "OrderNote_AddNewPaymentPattern";
                if (noteDetaisPatternName == null) continue;

                string str = new Money(payment.Amount, new Currency(purchaseOrder.BillingCurrency)).ToString();
                AddNoteToOrder(purchaseOrder, noteDetaisPatternName, payment.PaymentType.ToString(), str);
            }
        }
    }
}