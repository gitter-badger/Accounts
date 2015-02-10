using System.Web.Mvc;
using Accounts.Models;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;

namespace AccountsTests.Controllers
{
    [TestFixture]
    public class UpdateContactDetailsFixture : AccountControllerFixtureBase
    {

        [Test]
        public void When_updating_contact_details_client_is_called()
        {
            _mockCoreClient.UpdateContactDetails(Arg.Any<string>(), Arg.Any<ContactDetails>())
                .Returns(new Response<object> {Errored = true});

            _accountController.UpdateContactDetails(new ContactDetails());

            _mockCoreClient.Received(1).UpdateContactDetails(Arg.Any<string>(), Arg.Any<ContactDetails>());
        }

        [Test]
        public void When_client_returns_error_controler_response_is_not_successful()
        {
            _mockCoreClient.UpdateContactDetails(Arg.Any<string>(), Arg.Any<ContactDetails>())
                .Returns(new Response<object> {Errored = true});

            var response = _accountController.UpdateContactDetails(new ContactDetails());

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<JsonResult>());
            var result = (JsonResult) response;
            dynamic jObj = JObject.FromObject(result.Data);
            Assert.That(jObj.success.Value, Is.False);
        }

    }
}