using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebApi.Entities
{
    public class Post : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PostUrl { get; set; }
        public long NumberOfViews { get; set; }
        public JobType JobType { get; set; }
        public long MinMoney { get; set; }
        public long MaxMoney { get; set; }
        public MoneyType MoneyType { get; set; }
        public int? TimeExperience { get; set; }
        public DateTime? EndDate { get; set; }
        public ExperienceType ExperienceType { get; set; }
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }
        [ForeignKey(nameof(Level))]
        public int LevelId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<CompanyPostHashtag> CompanyPostHashtags { get; set; }
        public virtual Level Level { get; set; }
        public virtual ICollection<CV> CVs { get; set; }
    }

    public enum JobType
    {
        PartTime = 0,
        FullTime = 1
    }

    public enum ExperienceType
    {
        NoExperience = 0,
        Month = 1,
        Year = 2
    }

    public enum MoneyType
    {
        NoNegotiable = 0,
        Negotiable = 1,
    }
}
