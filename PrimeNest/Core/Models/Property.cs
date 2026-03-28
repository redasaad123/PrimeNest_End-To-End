using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Property 
    {
        [Key]
        public string Id { get; set; }
        public string? TypeContract { get; set; }
        public string? Type { get; set; }
        public string? Area { get; set; }
        public string? Price { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? MoreDescription { get; set; }
        public string? MainPhoto { get; set; }
        public List<string>? Photo { get; set; }
        public string user { get; set; }
        public bool State { get; set; }
        public bool? IsRequested { get; set; }
        public DateTime? date { get; set; }

    }
}
