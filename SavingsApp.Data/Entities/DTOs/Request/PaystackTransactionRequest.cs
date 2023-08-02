using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request
{
    public class PaystackTransactionRequest
    {
        public string Reference { get; private set; }
        public int AmountInKobo { get; set; }
        public string Email { get; set; }
        public string CallbackUrl { get; set; }

        public PaystackTransactionRequest()
        {
            Reference = GenerateUniqueReference();
        }

        private string GenerateUniqueReference()
        {
            string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            string randomString = Guid.NewGuid().ToString("N").Substring(0, 8);
            string reference = timestamp + randomString;

            return reference;
        }
    }
}
