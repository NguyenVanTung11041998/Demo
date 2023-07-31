using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Models
{
    public class UserCreateModel
    {
        [Required]
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        [Required]
        public string PassWord { get; set; }
       
    }
}
