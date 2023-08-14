namespace DemoWebApi.Entities
{
    public class Level : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
