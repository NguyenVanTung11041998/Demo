using DemoWebApi.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }

    }
}
