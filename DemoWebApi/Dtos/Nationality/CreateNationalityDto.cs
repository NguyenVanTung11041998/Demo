namespace DemoWebApi.Dtos.Nationality
{
    public class CreateNationalityDto
    {
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
