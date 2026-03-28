using Core.Models;
using ProjectApi.DTOS;

namespace ProjectApi.ChainOfResponsibility
{
    public class AreaFilter : PropertyFilter
    {
        public override IEnumerable<Property> Apply(IEnumerable<Property> query, PropertySearchDto filter)
        {
            if (filter.Area.HasValue)
                query = query.Where(p =>Convert.ToDouble( p.Area) >= filter.Area.Value);

            return base.Apply(query, filter);
        }
    }
}
