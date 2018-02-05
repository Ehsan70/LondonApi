using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandonApi.Models
{
    public class Room : Resource
    {
        // This doesnt have id propertu because gref would fucntions as a restfull ID 
        // Splitting the Entity model and Resource model, gives us the flexibility to controll exactly what would be returned to client

        public string Name { get; set; }

        public decimal Rate { get; set; }
    }
}
