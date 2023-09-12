namespace DemoWebApi.Dtos.Company
{
    public class UpdateCompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string LocationDescription { get; set; }
        public string Location { get; set; }
        public string FullNameCompany { get; set; }
        public string Website { get; set; }
        public int? MinScale { get; set; }
        public string Treatment { get; set; }
        public int? MaxScale { get; set; }
        public bool IsHot { get; set; }
        public DateTime? LastUpdateIsHotTime { get; set; }
        public string CompanyUrl { get; set; }
        public int NationalityId { get; set; }
        public int UserId { get; set; }
    }
}
