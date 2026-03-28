using Core.Models;
using ProjectApi.DTOS;

namespace ProjectApi.ChainOfResponsibility
{
    public class TypeFilter : PropertyFilter
    {
        public override IEnumerable<Property> Apply(IEnumerable<Property> query, PropertySearchDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Type))
                query = query.Where(p => p.Type.ToLower() == filter.Type.ToLower());

            return base.Apply(query, filter);
        }
    }
}
