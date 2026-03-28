using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectApi.NewFolder
{
    public class GetPropertyDTO
    {

        public string? Id { get; set; }
        public string? TypeContract { get; set; }
        public string? Type { get; set; }
        public string? Area { get; set; }
        public string? Price { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? MoreDescription { get; set; }
        public string? MainPhoto { get; set; }
        public List<string>? Photo { get; set; }
        public string? OwnerName { get; set; }
        public string? State { get; set; }
        public DateTime? date { get; set; }
    }
}
