namespace ProjectApi.DTOS
{
    public class AddRentDTO
    {
        public string PropertyID { get; set; }
        public string Price { get; set; }
        public string TenantId { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
