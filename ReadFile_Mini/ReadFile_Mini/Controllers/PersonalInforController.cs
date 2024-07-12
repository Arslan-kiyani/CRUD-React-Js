using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;
using ReadFile_Mini.Requestes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalInforController : ControllerBase
    {
        private readonly IpersonalInfoServicecs _personalInfoService;
        private readonly ILanguagesserver _languageService;
        private readonly IRaceservice _aceService;

        public PersonalInforController(IpersonalInfoServicecs personalInfoService, ILanguagesserver languagesserver, IRaceservice aceService)
        {
            _personalInfoService = personalInfoService;
            _languageService = languagesserver;
            _aceService = aceService;
        }

        [HttpPost("AddPersonalInfo")]
       
        public async Task<IActionResult> AddPersonalInfo([FromBody] personalRequest personalRequest)
        {
            try
            {
                bool addedSuccessfully = await _personalInfoService.AddPersonalAsync(personalRequest, personalRequest.Languages);
                if (addedSuccessfully)
                {
                    return Ok("PersonalInfo added successfully");
                }
                else
                {
                    return BadRequest("Personal information with the same email already exists.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding personal information.");
            }
        }

        [HttpGet("GetAllPersonalInfo")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PersonalInfo>>> GetAllPersonalInfo()
        {
            try
            {
                var personalInfo = await _personalInfoService.GetAllPersonalInfoAsync();
                return Ok(personalInfo);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetPersonalInfoById/{id}")]
        [ApiKey]
        public async Task<ActionResult<PersonalInfo>> GetPersonalInfoById(int id)
        {
            try
            {
                var personalInfo = await _personalInfoService.GetPersonalInfoByIdAsync(id);

                if (personalInfo == null)
                {
                    return NotFound("Personal info not found");
                }

                return Ok(personalInfo);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
      
        public async Task<IActionResult> DeletePersonalAsync(int id)
        {
            try
            {
                await _personalInfoService.DeletePersonalAsync(id);
                return Ok("Personal information deleted successfully.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Personal information not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting personal information.");
            }
        }

        [HttpPut("{id}")]
         
        public async Task<IActionResult> UpdatePersonalAsync(int id, [FromBody] personalRequest personalRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _personalInfoService.UpdatePersonalAsync(id, personalRequest);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Personal info not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
            return Ok(personalRequest);
        }

        [HttpGet("GetAllLanguages")]
        public async Task<ActionResult<IEnumerable<Language>>> GetAllLanguages()
        {
            try
            {
                var languages = await _languageService.GetAllLanguageAsync();
                return Ok(languages);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetAllRace")]
        public async Task<ActionResult<IEnumerable<Race>>> GetAllRace()
        {
            try
            {
                var race = await _aceService.GetAllRaceAsync();
                return Ok(race);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
