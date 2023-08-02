using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Response
{
    public class PaystackTransactionResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public TransactionData Data { get; set; }
        public string Reference { get; set; }
        
    }
}
