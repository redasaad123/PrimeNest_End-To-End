namespace ProjectApi.DTOS
{
    public class UpdateRentDTO
    {
        public string Price { get; set; }
        public string TenantId { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
