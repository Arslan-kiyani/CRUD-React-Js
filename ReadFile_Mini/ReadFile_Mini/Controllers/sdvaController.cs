using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Helper;
using ReadFile_Mini.Models;
using System.Text;

namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sdvaController : ControllerBase
    {
        private readonly sdvaHelper _sdvahelper;
        private readonly SeniorDb _seniorDb;
        public sdvaController(sdvaHelper sdvaHelper, SeniorDb seniorDb)
        {
            _sdvahelper = sdvaHelper;
            _seniorDb = seniorDb;

        }

        [HttpPost("Uploadsdva")]
        public async Task<IActionResult> Uploadsdva([FromForm] ExcelFileUploadModel file)
        {
            if (file == null || file.File.Length == 0)
            {
                return BadRequest("File is empty.");
            }
            // Check file extension
            string fileExtension = Path.GetExtension(file.File.FileName);
            if (string.IsNullOrEmpty(fileExtension) ||
                !(fileExtension.Equals(".xls", StringComparison.OrdinalIgnoreCase) ||
                  fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Invalid file format. Only Excel files (.xls, .xlsx) are allowed.");
            }

            string[] allowedMimeTypes = { "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
            if (!allowedMimeTypes.Contains(file.File.ContentType))
            {
                return BadRequest("Invalid MIME type. Only Excel files are allowed.");
            }

            List<sdva> dataList;
            using (var stream = new MemoryStream())
            {
                await file.File.CopyToAsync(stream);
                stream.Position = 0;
                dataList = await _sdvahelper.ReadExcel(stream);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSdvaFromDatabase(string nameFilter = null, int page = 1, int pageSize = 10)
        {
            try
            {
                
                // Retrieve data from the database with pagination and filtering
                IQueryable<sdva> query = _seniorDb.sdvas.AsQueryable();

                // Apply filtering if filters are provided
                if (!string.IsNullOrEmpty(nameFilter))
                {
                    query = query.Where(sdva => sdva.FName.Contains(nameFilter)
                                            || sdva.LName.Contains(nameFilter)
                                            || sdva.Department.Contains(nameFilter));
                }
                

                int totalItems = await query.CountAsync();
                List<sdva> dataList;

                // Check if data exists
                if (totalItems == 0)
                {
                    return NotFound("No data found in the database for the provided filters.");
                }

                // Execute paginated code if filtering parameters are provided
                dataList = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                // Generate CSV content if filtering parameters are provided
                if (!string.IsNullOrEmpty(nameFilter))
                {
                    StringBuilder csvContent = new StringBuilder();
                    csvContent.AppendLine("ID,EDI,FName,LName,Department,SSN,EMAIL_ADDRESS,GENDER,PHONE_NUMBER");

                    foreach (var item in dataList)
                    {
                        csvContent.AppendLine($"{item.id},{item.EDI},{item.FName},{item.LName},{item.Department},{item.SSN},{item.EMAIL_ADDRESS},{item.PHONE_NUMBER}");
                    }

                    // Return CSV file
                    byte[] data = Encoding.UTF8.GetBytes(csvContent.ToString());
                    return File(data, "text/csv", "sdva_data.csv");
                }
                else
                {
                    // If no filtering parameters provided, return paginated data without generating CSV
                    PaginatedData<sdva> paginatedData = new PaginatedData<sdva>
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalItems = totalItems,
                        Data = dataList
                    };

                    return Ok(paginatedData);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return HTTP 500 if an error occurs
            }
        }

    }


}
