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
    public class FeedBack
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("UserId")]

        public string UserId { get; set; }

        public string Message { get; set; }

        public DateTime? DateTime { get; set; }
        [JsonIgnore]
        public AppUser? User { get; set; }
        
    }
}
