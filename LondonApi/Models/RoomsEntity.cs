using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandonApi.Models
{
    public class RoomEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        // Price of the rroms in cents 
        public int Rate { get; set; }
    }
}
