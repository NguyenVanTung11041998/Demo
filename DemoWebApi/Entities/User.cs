using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; } 
        public string UserEmail { get; set; }
        public string PassWord { get; set; }
        public string? Avatar { get; set; }
    }
}
