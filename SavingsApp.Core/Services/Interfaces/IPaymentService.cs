using SavingsApp.Data.Entities.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Core.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ResponseDto<bool>> FundPersonalSavings(string senderWalletId, string personalSavingsId, double amount);
    }
}
