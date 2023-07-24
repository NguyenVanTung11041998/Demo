namespace DemoWebApi.Models
{
    public class StudentUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int ClassId { get; set; }
    }
}
