using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Models
{
    public class CategoryUpdateModel
    {
        [Required]
        public string Name { get; set; }
       
    }
}
