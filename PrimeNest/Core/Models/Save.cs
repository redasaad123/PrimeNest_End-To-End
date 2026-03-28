using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Save
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("PropertyId")]
        public string PropertyId { get; set; }

        [ForeignKey("UserId")]

        public string UserId { get; set; }

        public DateTime? DateTime { get; set; }
        [JsonIgnore]
        public Property? Property { get; set; }
        public AppUser? User { get; set; }
        
    }
}
