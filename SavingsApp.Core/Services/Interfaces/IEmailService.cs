using SavingsApp.Data.Entities.Models;

namespace MySchool.Core.Interface
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
