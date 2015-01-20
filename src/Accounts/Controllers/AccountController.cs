using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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
        [HttpPost]
        public ActionResult UpdatePassword(string password)
        {
            //validate password check password satisfies the complexity rules

            //call api to reset the password
            //get the current user
            var user = HttpContext.GetOwinContext().Authentication.User;

            if (user == null)
            {
                return Unauthorised();
            }

            var subClaim = user.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subClaim == null)
            {
                return Unauthorised();
            }

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("password", password)
            };

            var content = new FormUrlEncodedContent(pairs);

            var client = new HttpClient {BaseAddress = new Uri(EndPoints.IdentityApiAddress)};

            // call sync
            var response = client.PostAsync("/credentials/" + subClaim.Value, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return Json(new {success = true});
            }

            Response.StatusCode = (int) response.StatusCode;
            return Json(new { success = false });
        }

        private ActionResult Unauthorised()
        {
            Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            return Json(new {success = false});
        }
    }
}