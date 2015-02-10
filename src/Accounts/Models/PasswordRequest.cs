namespace Accounts.Models
{
    public class PasswordRequest
    {
        public string Password { get; set; }
    }

    public class ContactDetails
    {
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }

    public class ContactDetailsResponse
    {
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string PasswordType { get; set; }
    }

    public class Response<T>
    {
        public bool Errored { get; set; }
        public string ErrorMessage { get; set; }
        public T Body { get; set; }
    }
}