using ProjectApi.DTOS.InterfaceDTO;

namespace ProjectApi.DTOS
{
    public class UpdateVillaDTO : DetailsPropertyDTO
    {
        public int? NumberOfRooms { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public int? NumberStorey { get; set; }
        public string? PoolArea { get; set; }
        public string? gardenArea { get; set; }
    }
}
