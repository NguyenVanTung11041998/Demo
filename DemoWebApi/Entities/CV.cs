using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebApi.Entities
{
    public class CV : IEntity<int>
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int? UserId { get; set; }
        public string Link { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Portfolio { get; set; }
        public bool IsRead { get; set; }
        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
