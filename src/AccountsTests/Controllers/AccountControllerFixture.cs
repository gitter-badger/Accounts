using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Accounts.Clients;
using Accounts.Controllers;
using Microsoft.Owin.Security;
using NSubstitute;
using NUnit.Framework;

namespace AccountsTests.Controllers
{

    public abstract class AccountControllerFixtureBase
    {
        internal AccountController _accountController;
        internal IAuthenticationManager _mockAuthenticationManager;
        internal ICoreClient _mockCoreClient;

        [SetUp]
        public void SetUp()
        {

            _mockAuthenticationManager = Substitute.For<IAuthenticationManager>();
            _mockCoreClient = Substitute.For<ICoreClient>();

            SetupValidUser();

            _accountController = new AccountController(_mockAuthenticationManager, _mockCoreClient);

            var context = Substitute.For<HttpContextBase>();
            var requestContext = new RequestContext(context, new RouteData());
            _accountController.ControllerContext = new ControllerContext(requestContext, _accountController);
        }

        private void SetupValidUser()
        {
            var collection = new List<ClaimsIdentity>();
            var identity = new ClaimsIdentity(new DummyIdentity(), new[] { new Claim("sub", "1") });
            collection.Add(identity);
            var principal = new ClaimsPrincipal(collection);

            _mockAuthenticationManager.User.Returns(principal);
        }

    }


    internal class DummyIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get { return "Dummy"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return "DummyUser"; }
        }
    }



}
