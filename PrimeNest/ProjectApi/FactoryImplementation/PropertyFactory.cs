using Core.Models;
using ProjectApi.Factory;

namespace ProjectApi.FactoryImplementation
{
    public class PropertyFactory : IPropertyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PropertyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IDetailsProperty> DetailsProperty(string propertyType)
        {
            return propertyType.ToLower() switch
            {
                "apartment" => _serviceProvider.GetRequiredService<Apartment>(),
                "villa" => _serviceProvider.GetRequiredService<Villa>(),
                "floor" => _serviceProvider.GetRequiredService<Floor>(),
                _ => throw new ArgumentException("❌ نوع العقار غير معروف")
            };

             
        }


    }
}
