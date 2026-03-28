namespace ProjectApi.DTOS.InterfaceDTO
{
    public interface IPropertyDTO
    {
        public string? TypeContract { get; set; }
        public string? Type { get; set; }
        public string? Area { get; set; }
        public string? Price { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? MoreDescription { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public List<IFormFile>? Photo { get; set; }
        public string owner { get; set; }
    }
}
