using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request.PersonalSaving
{
    public class PersonalSavingsFundingRequest
    {
        public string SenderId { get; set; }
        public string SavingsId { get; set; }
        public double Amount { get; set; }
    }
}
