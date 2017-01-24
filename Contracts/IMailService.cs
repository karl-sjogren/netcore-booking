using System.Threading.Tasks;

namespace WebApplication.Contracts {
    public interface IMailService {
        Task SendMessage(string recipient, string subject, string body);
    }
}