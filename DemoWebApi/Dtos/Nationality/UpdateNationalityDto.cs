namespace DemoWebApi.Dtos.Nationality
{
    public class UpdateNationalityDto
    {
        public IFormFile File { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
