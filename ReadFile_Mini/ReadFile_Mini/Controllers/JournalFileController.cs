using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadFile_Mini.Helper;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalFileController : ControllerBase
    {
        private readonly JournalHelper _journalHelper;
        private readonly ExcelFileUploadHelper _excelFileUploadHelper;
        public JournalFileController(JournalHelper journalHelper, ExcelFileUploadHelper excelFileUploadHelper)
        {

            _journalHelper = journalHelper;
            _excelFileUploadHelper = excelFileUploadHelper;

        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ExcelFileUploadModel file)
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

            List<JournalByDate> dataList;
            using (var stream = new MemoryStream())
            {
                await file.File.CopyToAsync(stream);
                stream.Position = 0;
                dataList = await _journalHelper.ReadExcel(stream);
            }
            return Ok(dataList);
        }

        [HttpPost("ExcelFileUpload")]
        public async Task<IActionResult> ExcelFileUpload([FromForm] ExcelFileUploadModel file)
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

            List<ExcelsFiles> dataList;
            using (var stream = new MemoryStream())
            {
                await file.File.CopyToAsync(stream);
                stream.Position = 0;
                dataList = await _excelFileUploadHelper.ReadExcel(stream);
            }
            return Ok(dataList);
        }

    }
}
