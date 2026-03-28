using Core.Models;
using ProjectApi.DTOS;

namespace ProjectApi.ChainOfResponsibility
{
    public class KeywordFilter : PropertyFilter
    {
        public override IEnumerable<Property> Apply(IEnumerable<Property> query, PropertySearchDto filter)
        {
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(p =>
                    p.Description.Contains(filter.Keyword) ||
                    p.Address.Contains(filter.Keyword));
            }

            return base.Apply(query, filter);
        }

    }
}
