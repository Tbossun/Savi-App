namespace SavingsApp.Data.Entities.Models
{
    public class Occupation : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}