using System;
using Microsoft.Extensions.Configuration;
using WebApplication.Contracts;

namespace WebApplication.Services {
    public class AuthenticationStore : IAuthenticationStore {
        private readonly string _pin;

        public AuthenticationStore(IConfigurationRoot configuration) {
            _pin = configuration["AUTH_PIN"];
        }

        public bool ValidatePin(string pin) {
            if(_pin == null)
                return false;

            return _pin.Equals(pin, StringComparison.OrdinalIgnoreCase);
        }
    }
}