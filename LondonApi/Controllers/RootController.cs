using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LondonApi.Controllers
{
    // Tells the routing system to respond to root requests
    [Route("/")]
    [ApiVersion("1.0")]
    // [RequireHttps] This would make this controller encrypted.
    public class RootController : Controller
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new
            {
                // This will generate a absolute url. No route paramaters 
                href = Url.Link(nameof(GetRoot), null),
                rooms = new { href = Url.Link(nameof(RoomsController.GetRooms), null)}
            };

            return Ok(response);

        }

    }
}
