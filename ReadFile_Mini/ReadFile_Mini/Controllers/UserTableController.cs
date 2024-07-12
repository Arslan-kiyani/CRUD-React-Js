using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Requestes;

namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTableController : ControllerBase
    {

        private readonly IUserTableService _userTableService;

        public UserTableController(IUserTableService userTableService)
        {
            _userTableService = userTableService;
        }
        //[HttpPost]
        //public async Task<IActionResult> AddUser(UserTableRequest userTableRequest)
        //{
        //    await _userTableService.AddUserAsync(userTableRequest);
        //    return Ok("User added successfully");
        //}

        [HttpGet("GetAllUsersAsync")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var usersByTrip = await _userTableService.GetAllUsersAsync();
            if (usersByTrip == null || usersByTrip.Count == 0)
            {
                return NotFound();
            }
            return Ok(usersByTrip);
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userTableService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Return 404 if user is not found
            }
            return Ok(user); // Return 200 with user data  
          
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userTableService.DeleteUserAsync(id);
            return Ok(id); 
        }

        // PUT: /api/UserTable/UpdateUser/{userId}
        [HttpPut("UpdateUser")]

        public async Task<IActionResult> UpdateUser(int id, UserTableRequest userTableRequest)
        {
            // Call the service method to update the user
            await _userTableService.UpdateUserAsync(id, userTableRequest);
            return Ok("User updated successfully");
            
        }

    }
}
