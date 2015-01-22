using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Accounts.Controllers;
using Microsoft.Owin.Security;
using NSubstitute;
using NUnit.Framework;

namespace AccountsTests.Controllers
{
    [TestFixture]
    public class AccountControllerFixture
    {
        AccountController _accountController;
        IAuthenticationManager _mockAuthenticationManager;

        [SetUp]
        public void SetUp()
        {
            _mockAuthenticationManager = Substitute.For<IAuthenticationManager>();

            _accountController = new AccountController(_mockAuthenticationManager);
        }

        [Test]
        public void When_user_is_not_found_expect_unauthorised_response()
        {
            _mockAuthenticationManager.User.Returns((ClaimsPrincipal)null);
            
            var result = _accountController.Create();

            Assert.That(result, Is.TypeOf<HttpUnauthorizedResult>());
        }

    }
}
