using Core.Interfaces;
using Core.Models;
using ProjectApi.ChainOfResponsibility;
using ProjectApi.DTOS;

namespace Core.Servises
{
    public class SearchService
    {
        private readonly IUnitOfWork<Property> _propertyUnitOfWork;

        public SearchService(IUnitOfWork<Property> propertyUnitOfWork)
        {
            _propertyUnitOfWork = propertyUnitOfWork;
        }

        public async Task<List<Property>> SearchProperties(PropertySearchDto filter)
        {
            // Initialize filters
            var priceFilter = new PriceFilter();
            var locationFilter = new LocationFilter();
            var typeContractFilter = new TypeContractFilter();
            var typeFilter = new TypeFilter();
            var areaFilter = new AreaFilter();
            var keywordFilter = new KeywordFilter();
            var dateFilter = new DateFilter();

            // Chain filters
            priceFilter.SetNext(locationFilter);
            locationFilter.SetNext(typeContractFilter);
            typeContractFilter.SetNext(typeFilter);
            typeFilter.SetNext(areaFilter);
            areaFilter.SetNext(keywordFilter);
            keywordFilter.SetNext(dateFilter);

            // Retrieve all properties and apply filter chain
            var properties = await _propertyUnitOfWork.Entity.GetAllAsync();
            var filteredProperties = priceFilter.Apply(properties, filter).ToList();

            return filteredProperties;
        }
    }
}