namespace ProjectApi.DTOS
{
    public class GetRentsDTO
    {
        public string ID { get; set; }
        public string PropertyID { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
        public DateTime Date { get; set; }
        public string OwnerId { get; set; }
        public string TenantId { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
