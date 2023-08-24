namespace DemoWebApi.Dtos.HashTag
{
    public class UpdateHashTagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HashtagUrl { get; set; }
        public bool IsHot { get; set; }
    }
}
