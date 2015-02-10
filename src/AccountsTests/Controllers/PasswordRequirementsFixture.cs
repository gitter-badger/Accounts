using System;
using System.Web.Mvc;
using Accounts.Controllers;
using Microsoft.Owin.Security;
using NSubstitute;
using NUnit.Framework;

namespace AccountsTests.Controllers
{
    [TestFixture]
    public class PasswordRequirementsFixture
    {
        [Test]
        public void When_password_is_not_eight_chars_expect_error_response()
        {
            Assert.That(PasswordRequestModelValidator.ValidatePassword(null), Is.False);
            Assert.That(PasswordRequestModelValidator.ValidatePassword(String.Empty), Is.False);
            Assert.That(PasswordRequestModelValidator.ValidatePassword("!2*Yyy"), Is.False, "Too short");
            Assert.That(PasswordRequestModelValidator.ValidatePassword("11111AAaa"), Is.False, "Needs special characters");
            Assert.That(PasswordRequestModelValidator.ValidatePassword("AA1f1SS^^"), Is.False, "Needs Lower case letters");
            Assert.That(PasswordRequestModelValidator.ValidatePassword("aa1f1ss^^"), Is.False, "Needs upper case letters");
            Assert.That(PasswordRequestModelValidator.ValidatePassword("AA1f1ss^^"), Is.True);
        }
    }
}