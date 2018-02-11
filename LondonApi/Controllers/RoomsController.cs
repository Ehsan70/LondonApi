using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LandonApi;
using LandonApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LondonApi.Controllers
{
    // [controller] would automatical use Rooms
    [Route("/[controller]")]
    public class RoomsController : Controller
    {
        // Note that directly refrencing the db context inside the controller is not good pattern. Will refacter this later. 
        private readonly HotelApiContext _context;
        public RoomsController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet(Name =nameof(GetRooms))]
        public IActionResult GetRooms()
        {
            throw new NotImplementedException();
        }

        // /rooms/{roomId}
        [HttpGet("{roomId}", Name = nameof(GetRoomByIdAsync))]
        public async Task<IActionResult> GetRoomByIdAsync(Guid roomId, CancellationToken ct)
        {
            //  Cancellation token for Async method is good practise because asp.net core will 
            // automatically send a cancellation message if the browser or client cancells the request
            // pull out an entity from database taht matches this ID  
            var entity = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == roomId, ct);
            if (entity == null)
                return NotFound();

            // If the entity was found, we need to map the entity to into room resource 
            var resource = new Room
            {
                Href = Url.Link(nameof(GetRoomByIdAsync), new { roomId = entity.Id }),
                Name = entity.Name,
                Rate = entity.Rate / 100
            };
            return Ok(resource);
        }
    }
}
