using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.Models
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}
