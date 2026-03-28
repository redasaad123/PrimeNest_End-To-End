using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public abstract class TypeOp
    {
        [Key]
        public string ID { get; set; }
        public string PropertyID { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
        public DateTime Date { get; set; }
    }
}
