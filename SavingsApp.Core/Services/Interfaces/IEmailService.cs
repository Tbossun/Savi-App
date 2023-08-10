using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Core.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
