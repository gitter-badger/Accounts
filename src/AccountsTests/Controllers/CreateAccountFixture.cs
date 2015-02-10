using System.Security.Claims;
using System.Web.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AccountsTests.Controllers
{
    [TestFixture]
    public class CreateAccountFixture : AccountControllerFixtureBase
    {

        [Test]
        public void When_user_is_not_found_expect_unauthorised_response()
        {
            _mockAuthenticationManager.User.Returns((ClaimsPrincipal)null);
            
            var result = _accountController.Create();

            Assert.That(result, Is.TypeOf<HttpUnauthorizedResult>());
        }

        [Test]
        public void When_create_is_called_expect_details_to_be_retrieved()
        {
            _accountController.Create();

            _mockCoreClient.Received(1).GetContactDetails(Arg.Any<string>());
        }

    }
}