using System;
using System.Collections.Generic;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;
using Mediachase.Commerce.Orders;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface IOrdersService
    {
        List<OrderOrganizationViewModel> GetUserOrders(Guid userGuid);
        IPayment GetOrderBudgetPayment(IPurchaseOrder purchaseOrder);
        bool ApproveOrder(int orderGroupId);
        ContactViewModel GetPurchaserCustomer(OrderGroup order);
    }
}
