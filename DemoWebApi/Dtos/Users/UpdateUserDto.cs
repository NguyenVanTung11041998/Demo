namespace DemoWebApi.Dtos.Users
{
    public class UpdateUserDto
    {
        public IFormFile File { get; set; }
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Path { get; set; }
    }
}
