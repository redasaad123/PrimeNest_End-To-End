using ProjectApi.DTOS.InterfaceDTO;

namespace ProjectApi.DTOS
{
    public class GetTransactionDTO  
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string BuyerID { get; set; }
        public string SellerID { get; set; }
        public string PropertyID { get; set; }
        public string Price { get; set; }
        public DateTime Date { get; set; }



    }
}
