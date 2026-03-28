using ProjectApi.DTOS.InterfaceDTO;

namespace ProjectApi.DTOS
{
    public class UpdateApartmentDTO : DetailsPropertyDTO
    {
        public int? NumberOfRooms { get; set; }
        public int? NumberOfBathrooms { get; set; }

        public int? NumberStorey { get; set; }
    }
}
