using System.Web.Mvc;

namespace Accounts.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public ActionResult Reset()
        {
            return View();
        }
    }
}