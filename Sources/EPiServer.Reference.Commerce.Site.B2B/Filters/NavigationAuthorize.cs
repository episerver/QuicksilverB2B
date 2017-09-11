using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.B2B.Enums;
using EPiServer.Reference.Commerce.Site.B2B.Models.Contact;
using EPiServer.Web.Routing;
using EPiServer.Reference.Commerce.Site;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.B2B.Filters
{

    public class NavigationAuthorize : ActionFilterAttribute
    {
        private List<B2BUserRoles> _authorizedRoles;

        public NavigationAuthorize(string authorizedRoles)
        {
            ToB2BRoles(authorizedRoles);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ValidateUserRole()) return;

            var url = new UrlHelper(filterContext.RequestContext);
            var redirectUrl = url.Action("Index", "Start");
            filterContext.Result = new RedirectResult(redirectUrl);
        }

        private bool ValidateUserRole()
        {
            var contactRole = new B2BContact(CustomerContext.Current.CurrentContact).B2BUserRole;
            return _authorizedRoles.Any(role => contactRole == role);
        }

        private void ToB2BRoles(string authorizedRoles)
        {
            _authorizedRoles = new List<B2BUserRoles>();
            var roles = authorizedRoles.Split(',');
            foreach (var role in roles)
            {
                B2BUserRoles b2bRole;
                switch (role)
                {
                    case "Admin":
                        b2bRole = B2BUserRoles.Admin;
                        break;
                    case "Approver":
                        b2bRole = B2BUserRoles.Approver;
                        break;
                    case "Purchaser":
                        b2bRole = B2BUserRoles.Purchaser;
                        break;
                    default:
                        b2bRole = B2BUserRoles.None;
                        break;
                }
                _authorizedRoles.Add(b2bRole);
            }
            
        }
    }
}
