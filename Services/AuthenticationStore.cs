using Microsoft.Extensions.Configuration;
using WebApplication.Contracts;

namespace WebApplication.Services {
    public class AuthenticationStore : IAuthenticationStore {
        private readonly string _pin;

        public AuthenticationStore(IConfigurationRoot configuration) {
            _pin = configuration["AUTH_PIN"];
        }

        public string GetPin() {
            return _pin ?? string.Empty;
        }
    }
}