using Core.Interfaces;
using Core.Models;
using ProjectApi.DTOS;
using ProjectApi.DTOS.InterfaceDTO;
using ProjectApi.Factory;

namespace ProjectApi.FactoryImplementation
{
    public class Villa : IDetailsProperty
    {
        private readonly IUnitOfWork<Property> propertyUnitOfWork;
        private readonly IUnitOfWork<DetailsVilla> VillaUnitOfWork;

        public Villa(IUnitOfWork<Property> PropertyUnitOfWork, IUnitOfWork<DetailsVilla> VillaUnitOfWork)
        {
            propertyUnitOfWork = PropertyUnitOfWork;
            this.VillaUnitOfWork = VillaUnitOfWork;
        }

        public async Task<IEnumerable<DetailsProperty>> GetDetails()
        {
            var details = await VillaUnitOfWork.Entity.GetAllAsync();
            return details;
            
        }
        public async Task< DetailsProperty> GetDetailsById(string Id)
        {
            var detail = VillaUnitOfWork.Entity.Find(x=>x.PropertyId == Id);

            return detail;
        }

        public async Task<DetailsProperty> AddDetails(DetailsPropertyDTO dto )
        {
            

            var VaildProperty = await propertyUnitOfWork.Entity.GetAsync(dto.PropertyId);
            if (VaildProperty == null)
                return null;


            var villa = new DetailsVilla
            {
                Id = Guid.NewGuid().ToString(),
                PropertyId = dto.PropertyId,
                Type = dto.type,
                NumberStorey = dto.NumberStorey,
                NumberOfBathrooms = dto.NumberOfBathrooms,
                NumberOfRooms = dto.NumberOfRooms,
                gardenArea = dto.gardenArea,
                PoolArea = dto.PoolArea,

            };

            await VillaUnitOfWork.Entity.AddAsync(villa);
            VillaUnitOfWork.Save();
            return villa;

        }

        public async Task<DetailsProperty> UpdateProperty(DetailsPropertyDTO dto, string Id)
        {
            var details = await VillaUnitOfWork.Entity.GetAsync(Id);

            if (details == null)
                return null;

            details.NumberStorey = dto.NumberStorey;
            details.NumberOfRooms = dto.NumberOfRooms;
            details.NumberOfRooms = dto.NumberOfRooms;
            details.gardenArea = dto.gardenArea;
            details.PoolArea = dto.PoolArea;

            await VillaUnitOfWork.Entity.UpdateAsync(details);
            VillaUnitOfWork.Save();
            return details;


        }


        public async Task<string> DeleteDetails(string Id)
        {
            var details = await VillaUnitOfWork.Entity.GetAsync(Id);

            if (details == null)
                return null;

            VillaUnitOfWork.Entity.Delete(details);
            VillaUnitOfWork.Save();
            return "The Details Is Deleted";

        }



    }
}
