using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } 
        public string CreatedUserId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
