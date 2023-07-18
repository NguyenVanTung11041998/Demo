using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebApi.Entities
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [ForeignKey(nameof(ClassRoom))]
        public int ClassId { get; set; }
        public virtual ClassRoom ClassRoom { get; set; }
    }
}
