namespace DemoWebApi.Dtos.Users
{
    public class UserUpdateAvatarRequest
    {
        public IFormFile File { get; set; }

        public string LinkAvatar { get; set; }
    }
}
