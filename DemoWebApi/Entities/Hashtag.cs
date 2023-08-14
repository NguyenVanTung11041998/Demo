namespace DemoWebApi.Entities
{
    public class Hashtag : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HashtagUrl { get; set; }
        public bool IsHot { get; set; }
        public virtual ICollection<CompanyPostHashtag> CompanyPostHashtags { get; set; }
    }
}
