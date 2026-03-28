using Core.Models;
using ProjectApi.DTOS;

namespace ProjectApi.ChainOfResponsibility
{
    public abstract class PropertyFilter
    {
        protected PropertyFilter? _next;

        public void SetNext(PropertyFilter next)
        {
            _next = next;
            
        }
        public virtual IEnumerable<Property> Apply(IEnumerable<Property> query, PropertySearchDto filter)
        {
            return _next?.Apply(query, filter) ?? query;
        }

    }
}
