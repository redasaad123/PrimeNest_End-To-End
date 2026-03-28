using Core.Models;
using ProjectApi.DTOS;

namespace ProjectApi.ChainOfResponsibility
{
    public class DateFilter : PropertyFilter
    {
        public override IEnumerable<Property> Apply(IEnumerable<Property> query, PropertySearchDto filter)
        {
            if (filter.Date.HasValue)
            {
                query = query.Where(p => p.date >= filter.Date.Value);
            }

            return base.Apply(query, filter); // انتقل للفيلتر التالي في السلسلة
        }

    }
}
