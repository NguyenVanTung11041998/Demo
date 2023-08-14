using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebApi.Entities
{
    public class Company : IEntity<int>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string LocationDescription { get; set; }
        public string Location { get; set; }
        public string FullNameCompany { get; set; }
        public string Website { get; set; }
        public int? MinScale { get; set; }
        public string Treatment { get; set; }
        public int? MaxScale { get; set; }
        public bool IsHot { get; set; }
        public DateTime? LastUpdateIsHotTime { get; set; }
        public string CompanyUrl { get; set; }
        [ForeignKey(nameof(Nationality))]
        public int NationalityId { get; set; }
        public virtual Nationality Nationality { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<CompanyPostHashtag> CompanyPostHashtags { get; set; }
        public virtual ICollection<BranchJobCompany> BranchJobCompanies { get; set; }
    }
}
