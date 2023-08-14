using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebApi.Entities
{
    public class Asset : IEntity<int>
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }
        public string Path { get; set; }
        public FileType FileType { get; set; }
        public virtual Company Company { get; set; }
    }

    public enum FileType
    {
        Thumbnail = 0,
        Image = 1
    }
}
