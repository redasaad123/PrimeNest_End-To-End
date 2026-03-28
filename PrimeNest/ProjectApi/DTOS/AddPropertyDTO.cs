using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectApi.DTOS.InterfaceDTO;

namespace ProjectApi.DTOS
{
    public class AddPropertyDTO : IPropertyDTO
    {
        public string? TypeContract { get; set; }
        public string? Type { get; set; }
        public string? Area { get; set; }
        public string? Price { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? MoreDescription { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public List<IFormFile>? Photo { get; set; }
        public string? owner { get; set; }

    }
}
