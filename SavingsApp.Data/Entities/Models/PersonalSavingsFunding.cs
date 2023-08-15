using SavingsApp.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.Models
{
    public class PersonalSavingsFunding 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public ActionType ActionType { get; set; }
        public decimal Amount { get; set; }
        public decimal CumulativeAmount { get; set; }
        public string Description { get; set; }
        public string personalSavingId { get; set; }
        public PersonalSaving personalSaving { get; set; }
    }
}
