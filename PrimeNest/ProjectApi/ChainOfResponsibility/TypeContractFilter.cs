using Core.Models;
using ProjectApi.DTOS;

namespace ProjectApi.ChainOfResponsibility
{
    public class TypeContractFilter : PropertyFilter
    {
        public override IEnumerable<Property> Apply(IEnumerable<Property> query, PropertySearchDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.TypeContract))
                query = query.Where(p => p.TypeContract.ToLower() == filter.TypeContract.ToLower());

            return base.Apply(query, filter);
        }
    }
}
