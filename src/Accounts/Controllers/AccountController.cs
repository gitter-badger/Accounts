using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Net.Http;
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

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("password", "password")
            };

            var content = new FormUrlEncodedContent(pairs);

            var client = new HttpClient { BaseAddress = new Uri("https://core-api.azurewebsites.net") };

            // call sync
            var response = client.PutAsync("/reset/1", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return Json(new {success = true});
            }
            Response.StatusCode = (int)response.StatusCode;
            return Json(new { success = false });
        }
    }
}