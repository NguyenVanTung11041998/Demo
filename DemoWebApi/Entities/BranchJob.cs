namespace DemoWebApi.Entities
{
    public class BranchJob : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BranchJobUrl { get; set; }
        public virtual ICollection<BranchJobCompany> BranchJobCompanies { get; set; }
    }
}
