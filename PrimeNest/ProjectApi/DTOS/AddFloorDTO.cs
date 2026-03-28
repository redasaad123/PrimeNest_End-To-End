using ProjectApi.DTOS.InterfaceDTO;
using ProjectApi.Factory;

namespace ProjectApi.DTOS
{
    public class AddFloorDTO : DetailsPropertyDTO
    {
        public string PropertyId { get; set; }
        public string? TypeFloor { get; set; }
    }
}
