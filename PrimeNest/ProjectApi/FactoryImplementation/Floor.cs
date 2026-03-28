using Core.Interfaces;
using Core.Models;
using ProjectApi.DTOS;
using ProjectApi.DTOS.InterfaceDTO;
using ProjectApi.Factory;

namespace ProjectApi.FactoryImplementation
{
    public class Floor : IDetailsProperty
    {
        private readonly IUnitOfWork<Property> PropertyUnitOfWork;
        private readonly IUnitOfWork<DetailsFloor> floorUnitOfWork;

        public Floor(IUnitOfWork<Property> PropertyUnitOfWork, IUnitOfWork<DetailsFloor> floorUnitOfWork)
        {
            this.PropertyUnitOfWork = PropertyUnitOfWork;
            this.floorUnitOfWork = floorUnitOfWork;
        }

        public async Task<DetailsProperty> GetDetailsById(string Id)
        {
            var detail = await floorUnitOfWork.Entity.GetAsync(Id);

            return detail;
        }

        public async Task<IEnumerable<DetailsProperty>> GetDetails()
        {
            var details = await floorUnitOfWork.Entity.GetAllAsync();

            return details;
        }


        public async Task<DetailsProperty> AddDetails(DetailsPropertyDTO DTO)
        {
            var VaildProperty = await PropertyUnitOfWork.Entity.GetAsync(DTO.PropertyId);
            if (VaildProperty == null)
                return null;


            var floor = new DetailsFloor
            {
                Id = Guid.NewGuid().ToString(),
                PropertyId = DTO.PropertyId,
                Type = DTO.type,
                TypeFloor = DTO.TypeFloor,

            };

            await floorUnitOfWork.Entity.AddAsync(floor);
            floorUnitOfWork.Save();
            return floor;

        }

        public async Task<DetailsProperty> UpdateProperty(DetailsPropertyDTO dto, string Id)
        {
            var details = await floorUnitOfWork.Entity.GetAsync(Id);

            if (details == null)
                return null;


            details.TypeFloor = dto.TypeFloor;
            await floorUnitOfWork.Entity.UpdateAsync(details);
            floorUnitOfWork.Save();
            return details;


        }

        public async Task<string> DeleteDetails(string Id)
        {
            var details = await floorUnitOfWork.Entity.GetAsync(Id);

            if (details == null)
                return null;

            floorUnitOfWork.Entity.Delete(details);
            floorUnitOfWork.Save();
            return "The Details Is Deleted";

        }


    }
}
