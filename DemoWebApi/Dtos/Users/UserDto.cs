namespace DemoWebApi.Dtos.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }
}
