﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace Accounts.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        [Authorize]
        public ActionResult Create()
        {
            if (_authenticationManager.User != null)
            {
                var client = new HttpClient {BaseAddress = new Uri(EndPoints.IdentityApiAddress)};

                var subClaim = _authenticationManager.User.Claims.FirstOrDefault(c => c.Type == "sub");

                if (subClaim != null)
                {
                    // call sync
                    var response = client.GetAsync("/users/" + subClaim.Value).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var obj = JsonConvert.DeserializeAnonymousType(response.Content.ReadAsStringAsync().Result,
                            new {email = "", passwordType = "", totalLogins = 0, mobileNumber = ""});
                        return View(new
                        {
                            PasswordSet = !String.IsNullOrWhiteSpace(obj.passwordType) && obj.passwordType.ToLowerInvariant() != "firsttime",
                            PrimaryEmailExists = !String.IsNullOrWhiteSpace(obj.email),
                            PrimaryPhoneExists = !String.IsNullOrWhiteSpace(obj.mobileNumber)
                        });
                    }
                    else
                    {
                        return new HttpStatusCodeResult(response.StatusCode);
                    }
                }

            }
            return new HttpUnauthorizedResult("Not authorized");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdatePassword(string password)
        {
            //validate password check password satisfies the complexity rules

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

        [Authorize]
        [HttpPost]
        public ActionResult UpdateContactDetails(ContactDetails contactDetails)
        {
            //validate password check password satisfies the complexity rules

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

            var contactString = Newtonsoft.Json.JsonConvert.SerializeObject(contactDetails,
                Newtonsoft.Json.Formatting.None,
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                });

            var content = new StringContent(contactString, Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(EndPoints.IdentityApiAddress) };
            
            // call sync
            var response = PatchAsync(client, "/users/" + subClaim.Value, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }

            Response.StatusCode = (int)response.StatusCode;
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

        Task<HttpResponseMessage> PatchAsync(HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            return client.SendAsync(request);
        }

    }

    public class ContactDetails
    {
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }
}