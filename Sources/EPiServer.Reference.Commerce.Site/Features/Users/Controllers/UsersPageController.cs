using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.B2B.Models.Pages;
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