namespace SavingsApp.Data.Entities.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public virtual ICollection<PersonalSaving> personalSavings { get; set; }
    }
}