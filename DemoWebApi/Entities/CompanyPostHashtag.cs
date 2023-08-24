using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebApi.Entities
{
    public class CompanyPostHashtag : IEntity<int>
    {
        public int Id { get; set; }
        [ForeignKey(nameof(HashTag))]
        public int HashtagId { get; set; }
        [ForeignKey(nameof(Company))]
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(Post))]
        public int? PostId { get; set; }
        public virtual Company Company { get; set; }
        public virtual Post Post { get; set; }
        public virtual HashTag HashTag { get; set; }
    }
}
