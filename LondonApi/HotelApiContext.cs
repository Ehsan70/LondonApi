using LandonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LandonApi
{
    public class HotelApiContext : DbContext
    {
        // This passes the options class to base contructor 
        public HotelApiContext(DbContextOptions options)
            : base(options) { }

        // DbSets would be tables for room entity
        public DbSet<RoomEntity> Rooms { get; set; }
    }
}
