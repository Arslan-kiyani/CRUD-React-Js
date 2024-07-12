﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadFile_Mini.Context;
using ReadFile_Mini.Helper;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
       
        private readonly InventoryHelper _inventoryHelper;
        public InventoryController( InventoryHelper inventoryHelper)
        {
            
            _inventoryHelper = inventoryHelper;

        }

        [HttpPost("UploadInventory")]
        public async Task<IActionResult> UploadInventory([FromForm] ExcelFileUploadModel file)
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

            List<Inventory> dataList;
            using (var stream = new MemoryStream())
            {
                await file.File.CopyToAsync(stream);
                stream.Position = 0;
                dataList = await _inventoryHelper.ReadExcel(stream);
            }
            return Ok();
        }
    }
}
