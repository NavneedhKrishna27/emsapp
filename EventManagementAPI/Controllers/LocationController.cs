using EventManagementSystemMerged.Models;
using EventManagementSystemMerged.Repos;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;

        public LocationController()
        {
            _locationService = new LocationService();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Location>> GetLocations()
        {
            var locations = _locationService.GetLocationDetails();
            return Ok(locations);
        }

        [HttpPost]
        public ActionResult AddLocation([FromBody] Location location)
        {
            _locationService.EnterLocationDetails(location);
            return CreatedAtAction(nameof(GetLocationById), new { id = location.LocationID }, location);
        }

        [HttpGet("{id}")]
        public ActionResult<Location> GetLocationById(int id)
        {
            var location = _locationService.GetLocationDetails().FirstOrDefault(l => l.LocationID == id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [HttpPut("{id}")]
        public ActionResult EditLocation(int id, [FromBody] Location location)
        {
            if (id != location.LocationID)
            {
                return BadRequest();
            }

            _locationService.UpdateLocationDetails(location, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteLocation(int id)
        {
            var location = _locationService.GetLocationDetails().FirstOrDefault(l => l.LocationID == id);
            if (location == null)
            {
                return NotFound();
            }

            _locationService.RemoveLocationDetails(id);
            return NoContent();
        }
    }
}
