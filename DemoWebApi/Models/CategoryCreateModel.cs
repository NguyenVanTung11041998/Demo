using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Models
{
    public class CategoryCreateModel
    {
        [Required]
        public string Name { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
       
    }
}
