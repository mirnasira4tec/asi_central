using System;

namespace asi.asicentral.model
{
    public class Security
    {
        public string Password { get; set; }
        public string PasswordHint { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Password);
        }
    }
}
