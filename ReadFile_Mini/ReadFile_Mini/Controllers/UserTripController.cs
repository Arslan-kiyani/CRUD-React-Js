using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Requestes;

namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTripController : ControllerBase
    {
        private readonly IUserTripService _userTripService;

        public UserTripController(IUserTripService userTripService)
        {
            _userTripService = userTripService;
        }

        [HttpPost("AddUserTrip")]
        public async Task<IActionResult> AddUserTrip(UserTripRequest userTripRequest)
        {
            await _userTripService.AddUserTripAsync(userTripRequest);
            return Ok("User trip added successfully");
            
        }

        // GET: /api/UserTrip
        //[HttpGet("GetAllUserTrips")]
        //public async Task<IActionResult> GetAllUserTrips()
        //{
        //    var userTripResponses = await _userTripService.GetAllUserTripsAsync();
        //    return Ok(userTripResponses); 
        //}

        // GET: /api/UserTrip/{id}
        //[HttpGet("userTripId")]
        //public async Task<ActionResult<UserTripResponse>> GetUserTripById(int userTripId)
        //{
        //    var userTrip = await _userTripService.GetUserTripByIdAsync(userTripId);

        //    if (userTrip == null)
        //    {
        //        return NotFound(); // Return 404 if userTrip is not found
        //    }

        //    return Ok(userTrip); // Return 200 OK with the userTripResponse
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTrip(int id)
        {
            await _userTripService.DeleteUserTripAsync(id);
            return Ok("User trip deleted successfully.");
          
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserTrip(int id, UserTripRequest userTripRequest)
        {
                var updatedUserTrip = await _userTripService.UpdateUserTripAsync(id, userTripRequest);
                if (updatedUserTrip != null)
                {
                    return Ok(updatedUserTrip);
                }
                else
                {
                    return NotFound($"User trip with ID {id} not found.");
                }
        }
    }
}
