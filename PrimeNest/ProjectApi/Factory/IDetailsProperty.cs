using Core.Models;
using ProjectApi.DTOS;
using ProjectApi.DTOS.InterfaceDTO;


namespace ProjectApi.Factory
{
    public interface IDetailsProperty
    {
        Task<IEnumerable<DetailsProperty>> GetDetails();
      Task<  DetailsProperty> GetDetailsById(string Id);

        Task<DetailsProperty> AddDetails(DetailsPropertyDTO dto);

        Task<DetailsProperty> UpdateProperty(DetailsPropertyDTO dto, string Id);

        Task<string> DeleteDetails(string Id);
    }
}
