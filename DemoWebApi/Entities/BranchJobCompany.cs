using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebApi.Entities
{
    public class BranchJobCompany : IEntity<int>
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }
        [ForeignKey(nameof(BranchJob))]
        public int BranchJobId { get; set; }
        public virtual Company Company { get; set; }
        public virtual BranchJob BranchJob { get; set; }
    }
}
