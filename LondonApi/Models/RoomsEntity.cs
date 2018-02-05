using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandonApi.Models
{
    // yo ucan use simple classes to represent your database models/entities. 
    // These are called POCOS or Plain Old CLR objects
    public class RoomEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        // Price of the rooms in cents. We'll divide by 100 later
        public int Rate { get; set; }
    }
}
