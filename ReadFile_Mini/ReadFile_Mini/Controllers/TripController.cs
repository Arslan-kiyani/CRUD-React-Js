using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Requestes;


namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTrip(TripRequest tripRequest)
        {
            await _tripService.AddTripAsync(tripRequest);
            return Ok("Trip added successfully");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTrips()
        {
            try
            {
                var trips = await _tripService.GetAllTripsAsync();
                return Ok(trips);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while retrieving trips");
            }
        }

        [HttpGet("GetAllPostGres")]
        public async Task<IActionResult> GetAllPostGres()
        {
            try
            {
                var dis = await _tripService.GetAllPostGresAsync();
                return Ok(dis);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving Data");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null)
            {
                return NotFound("Trip not found");
            }
            return Ok(trip);
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteTrip(int id)
        //{
        //    await _tripService.DeleteTripAsync(id);
        //    return Ok(id);
        //}

        // PUT: api/Trip/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, TripRequest tripRequest)
        {
            
            await _tripService.UpdateTripAsync(id, tripRequest);
            return Ok("Trip updated successfully");
            
           
        }

    }
}
