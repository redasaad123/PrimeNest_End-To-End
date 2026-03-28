using Core.Models;

namespace ProjectApi.Factory
{
    public interface IPropertyFactory
    {
       Task<IDetailsProperty> DetailsProperty(string propertyType);
    }
}
