using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Accounts.Binders;
using Accounts.Clients;
using Accounts.Models;
using Microsoft.Owin.Security;

namespace Accounts.Controllers
{
    public class AccountController : Controller
    {
        readonly IAuthenticationManager _authenticationManager;
        readonly ICoreClient _coreClient;

        public AccountController(IAuthenticationManager authenticationManager, ICoreClient coreClient)
        {
            _authenticationManager = authenticationManager;
            _coreClient = coreClient;
        }

        [Authorize]
        public ActionResult Create()
        {
            if (_authenticationManager.User != null)
            {
                var subClaim = _authenticationManager.User.Claims.FirstOrDefault(c => c.Type == "sub");

                if (subClaim != null)
                {
                    // call sync
                    var response = _coreClient.GetContactDetails(subClaim.Value);
                    if (response != null && !response.Errored && response.Body != null)
                    {
                        return View(new
                        {
                            PasswordSet = !String.IsNullOrWhiteSpace(response.Body.PasswordType) && response.Body.PasswordType.ToLowerInvariant() != "firsttime",
                            PrimaryEmailExists = !String.IsNullOrWhiteSpace(response.Body.Email),
                            PrimaryPhoneExists = !String.IsNullOrWhiteSpace(response.Body.MobileNumber)
                        });
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            }
            return new HttpUnauthorizedResult("Not authorized");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdatePassword([ModelBinder(typeof(PasswordRequestModelBinder))]PasswordRequest passwordRequest)
        {
            //validate password check password satisfies the complexity rules
            if (!this.ModelState.IsValid)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(new {success = false, errorMessage = "invalid request"});
            }

            //call api to reset the password
            //get the current user
            var user = _authenticationManager.User;

            if (user == null)
            {
                return UnauthorisedJson();
            }

            var subClaim = user.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subClaim == null)
            {
                return UnauthorisedJson();
            }

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("password", passwordRequest.Password)
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

        [Authorize]
        [HttpPost]
        public ActionResult UpdateContactDetails(ContactDetails contactDetails)
        {
            //call api to reset the password
            //get the current user
            var user = _authenticationManager.User;

            if (user == null)
            {
                return UnauthorisedJson();
            }

            var subClaim = user.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subClaim == null)
            {
                return UnauthorisedJson();
            }

            var response = _coreClient.UpdateContactDetails(subClaim.Value, contactDetails);
            if (!response.Errored)
            {
                return Json(new { success = true });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { success = false });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Continue(ContactDetails contactDetails)
        {
            return Redirect(EndPoints.PeopleCloudAddress);
        }

        private ActionResult UnauthorisedJson()
        {
            Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            return Json(new {success = false});
        }
    }



}