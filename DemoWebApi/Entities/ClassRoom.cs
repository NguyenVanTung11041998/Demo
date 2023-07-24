using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Entities
{
    public class ClassRoom
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
