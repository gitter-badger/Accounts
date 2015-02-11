using System.Web.Mvc;
using Accounts.Models;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;

namespace AccountsTests.Controllers
{
    [TestFixture]
    public class UpdatePasswordFixture : AccountControllerFixtureBase
    {
        [Test]
        public void When_update_is_called_client_is_called()
        {
            _mockCoreClient.UpdatePassword(Arg.Any<string>(), Arg.Any<PasswordRequest>()).Returns(new Response<object> { Errored = false });

            _accountController.UpdatePassword(new PasswordRequest());

            _mockCoreClient.Received(1).UpdatePassword(Arg.Any<string>(), Arg.Any<PasswordRequest>());
        }

        [Test]
        public void When_remote_call_fails_response_is_unsuccessful()
        {
            _mockCoreClient.UpdatePassword(Arg.Any<string>(), Arg.Any<PasswordRequest>()).Returns(new Response<object> { Errored = true });

            var response = _accountController.UpdatePassword(new PasswordRequest());

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<JsonResult>());
            var result = (JsonResult)response;
            dynamic jObj = JObject.FromObject(result.Data);
            Assert.That(jObj.success.Value, Is.False);
        }
    }
}