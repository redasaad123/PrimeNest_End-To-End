using ProjectApi.DTOS.InterfaceDTO;

namespace ProjectApi.DTOS
{
    public class AddTransactionDTO 
    {
        public string BuyerID { get; set; }
        public string SellerID { get; set; }
        public string PropertyID { get; set; }
        public string Price { get; set; }




    }
}
