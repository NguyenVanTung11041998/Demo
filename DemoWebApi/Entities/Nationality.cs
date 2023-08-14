namespace DemoWebApi.Entities
{
    public class Nationality : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}
