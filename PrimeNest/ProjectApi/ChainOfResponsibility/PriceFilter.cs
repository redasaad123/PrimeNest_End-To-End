using Core.Models;
using ProjectApi.DTOS;

namespace ProjectApi.ChainOfResponsibility
{
    public class PriceFilter : PropertyFilter
    {
        public override IEnumerable<Property> Apply(IEnumerable<Property> query, PropertySearchDto filter)
        {
            if (filter.Price.HasValue)
                query = query.Where(p => Convert.ToDouble(p.Price) >= filter.Price.Value);
            return base.Apply(query, filter);
        }

    }
}
