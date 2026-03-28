using Core.Interfaces;
using Core.Models;
using ProjectApi.DTOS;
using ProjectApi.DTOS.InterfaceDTO;
using ProjectApi.Factory;

namespace ProjectApi.FactoryImplementation
{
    public class Apartment : IDetailsProperty
    {
        private readonly IUnitOfWork<Property> propertyUnitOfWork;
        private readonly IUnitOfWork<DetailsApartment> ApartmentUnitOfWork;

        public Apartment(IUnitOfWork<Property> PropertyUnitOfWork, IUnitOfWork<DetailsApartment> ApartmentUnitOfWork)
        {
            propertyUnitOfWork = PropertyUnitOfWork;
            this.ApartmentUnitOfWork = ApartmentUnitOfWork;
        }

        public async Task<IEnumerable<DetailsProperty>> GetDetails()
        {
            var details = await ApartmentUnitOfWork.Entity.GetAllAsync();
            return details;
           
        }
        public async Task<DetailsProperty> GetDetailsById(string Id)
        {
            var detail = await ApartmentUnitOfWork.Entity.GetAsync(Id);

            return detail;
        }


        public async Task<DetailsProperty> AddDetails(DetailsPropertyDTO dto)
        {
            

            var VaildProperty = await propertyUnitOfWork.Entity.GetAsync(dto.PropertyId);
            if (VaildProperty == null)
                return null;

            var Apartment = new DetailsApartment
            {
                Id = Guid.NewGuid().ToString(),
                PropertyId = dto.PropertyId,
                Type = dto.type,
                NumberStorey = dto.NumberStorey,
                NumberOfBathrooms = dto.NumberOfBathrooms,
                NumberOfRooms = dto.NumberOfRooms,

            };

            await ApartmentUnitOfWork.Entity.AddAsync(Apartment);
            ApartmentUnitOfWork.Save();
            return Apartment;
           
        }

        public async Task<DetailsProperty> UpdateProperty(DetailsPropertyDTO dto , string Id)
        {
            var details = await ApartmentUnitOfWork.Entity.GetAsync(Id);

            if (details == null)
                return null;

            details.NumberStorey = dto.NumberStorey;
            details.NumberOfRooms = dto.NumberOfRooms;
            details.NumberOfBathrooms = dto.NumberOfBathrooms;

            await ApartmentUnitOfWork.Entity.UpdateAsync(details);
            ApartmentUnitOfWork.Save();
            return details;


        }

        public async Task<string> DeleteDetails(string Id)
        {
            var details = await ApartmentUnitOfWork.Entity.GetAsync(Id);

            if (details == null)
                return null;

            ApartmentUnitOfWork.Entity.Delete(details);
            ApartmentUnitOfWork.Save();
            return "The Details Is Deleted";

        }


    }
}
