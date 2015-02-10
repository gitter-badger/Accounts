using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Accounts.Binders
{
    public class PasswordRequestModelBinder : DefaultModelBinder
    {
        protected override bool OnPropertyValidating(ControllerContext controllerContext, ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.Name == "Password")
            {
                var val = value as string;
                if (ValidatePassword(val))
                {
                    return true;
                }
                bindingContext.ModelState.AddModelError("Password", "Does not meet password compliance requirements");
                return false;
            }
            return true;
        }

        public static bool ValidatePassword(string val)
        {
            return !String.IsNullOrWhiteSpace(val)
                   && val.Length >= 8
                   && val.Length <= 64
                   && Regex.IsMatch(val, "[a-z].*[A-Z]|[A-Z].*[a-z]")
                   && Regex.IsMatch(val, "\\d")
                   && Regex.IsMatch(val, "[-!$%^&*()_+|~=`{}\\[\\]:\";'<>?,.\\/]");
        }
    }
}