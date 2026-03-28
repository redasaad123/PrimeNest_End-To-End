using ProjectApi.DTOS.InterfaceDTO;

namespace ProjectApi.DTOS
{
    public class AddApartmentDTO : DetailsPropertyDTO
    {
        public string PropertyId { get; set; }
        public int? NumberOfRooms { get; set; }
        public int? NumberOfBathrooms { get; set; }

        public int? NumberStorey { get; set; }
    }
}
