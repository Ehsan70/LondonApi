using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LondonApi.Controllers
{
    // [controller] would automatical use Rooms
    [Route("/[controller]")]
    public class RoomsController : Controller
    {
        // GET: api/Rooms
        [HttpGet(Name =nameof(GetRooms))]
        public IActionResult GetRooms()
        {
            throw new NotImplementedException();
        }

    }
}
