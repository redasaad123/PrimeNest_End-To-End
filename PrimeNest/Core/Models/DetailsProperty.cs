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
    //dublicate code 
    public abstract class DetailsProperty
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("PropertyId")]
        public string Type { get; set; }
        public string PropertyId { get; set; }
        [JsonIgnore]
        public Property Property { get; set; }



    }
}
