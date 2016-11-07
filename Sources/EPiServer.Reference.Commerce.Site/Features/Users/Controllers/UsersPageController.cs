using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Users.Pages;
using EPiServer.Reference.Commerce.Site.Features.Users.ViewModels;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Users.Controllers
{
    [Authorize]
    public class UsersPageController : PageController<UsersPage>
    {
        public ActionResult Index(UsersPage currentPage)
        {
            var viewModel = new UsersPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
        public ActionResult AddUser(UsersPage currentPage)
        {
            var viewModel = new UsersPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
        public ActionResult EditUser(UsersPage currentPage)
        {
            var viewModel = new UsersPageViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
    }
}