using Core.Models;
using ProjectApi.DTOS;

namespace ProjectApi.ChainOfResponsibility
{
    public class LocationFilter : PropertyFilter
    {
        public override IEnumerable<Property> Apply(IEnumerable<Property> query, PropertySearchDto filter)
        {
            if (!string.IsNullOrEmpty(filter.Location))
                query = query.Where(p => p.Address.Contains(filter.Location));

            return base.Apply(query, filter);
        }
    }
}
