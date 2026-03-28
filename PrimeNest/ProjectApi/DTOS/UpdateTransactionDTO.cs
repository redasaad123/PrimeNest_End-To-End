using ProjectApi.DTOS.InterfaceDTO;

namespace ProjectApi.DTOS
{
    public class UpdateTransactionDTO 
    {
        public string? Price { get; set; }
        public string? BuyerID { get; set; }
        public string? PropertyID { get; set; }
        public string? Message { get; set; }
    }
}
