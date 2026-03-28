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
    public class Rent : TypeOp
    {
        [ForeignKey("OwnerId")]
        public string OwnerId { get; set; }
        public string TenantId { get; set; }
        public DateTime? DateEnd { get; set; }
        [JsonIgnore]
        public AppUser? User { get; set; }

    }
}
