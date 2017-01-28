namespace WebApplication.Contracts {
    public interface IAuthenticationStore {
        bool ValidatePin(string pin);
    }
}