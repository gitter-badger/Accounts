using System;
using System.Web.Mvc;
using Accounts.Binders;
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
            Assert.That(PasswordRequestModelBinder.ValidatePassword(null), Is.False);
            Assert.That(PasswordRequestModelBinder.ValidatePassword(String.Empty), Is.False);
            Assert.That(PasswordRequestModelBinder.ValidatePassword("!2*Yyy"), Is.False, "Too short");
            Assert.That(PasswordRequestModelBinder.ValidatePassword("11111AAaa"), Is.False, "Needs special characters");
            Assert.That(PasswordRequestModelBinder.ValidatePassword("AA1F1SS^^"), Is.False, "Needs Lower case letters");
            Assert.That(PasswordRequestModelBinder.ValidatePassword("aa1f1ss^^"), Is.False, "Needs Upper case letters");
            Assert.That(PasswordRequestModelBinder.ValidatePassword("AA1f1ss^^"), Is.True);
        }
    }
}