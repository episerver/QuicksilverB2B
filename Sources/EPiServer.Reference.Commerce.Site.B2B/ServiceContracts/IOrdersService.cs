using System;
using System.Collections.Generic;
using EPiServer.Reference.Commerce.Site.B2B.Models.ViewModels;

namespace EPiServer.Reference.Commerce.Site.B2B.ServiceContracts
{
    public interface IOrdersService
    {
        void GetUserOrders(Guid userGuid, out List<OrderOrganizationViewModel> ordersOrganization);
    }
}
