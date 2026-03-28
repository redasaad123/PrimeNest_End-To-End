using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DetailsApartment : DetailsProperty
    {
        public int? NumberOfRooms { get; set; }
        public int? NumberOfBathrooms { get; set; }

        public int? NumberStorey { get; set; }
    }
}
