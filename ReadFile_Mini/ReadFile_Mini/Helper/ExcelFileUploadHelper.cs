using Microsoft.EntityFrameworkCore.Query;
using MiniExcelLibs;
using ReadFile_Mini.Context;
using ReadFile_Mini.Models;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReadFile_Mini.Helper
{
    public class ExcelFileUploadHelper
    {
        private readonly SeniorDb _eniorDb;
        public ExcelFileUploadHelper(SeniorDb eniorDb)
        {
            _eniorDb = eniorDb;
        }

        public async Task<List<ExcelsFiles>> ReadExcel(Stream fileStream)
        {
            List<ExcelsFiles> dataList = new List<ExcelsFiles>();

            var rows = MiniExcel.Query(fileStream).ToList();
            if (rows == null || rows.Count == 0)
            {
                throw new Exception("No worksheet found in the Excel file or worksheet is empty.");
            }

            var firstRow = (IDictionary<string, object>)rows[0];
            string hotelName = firstRow.ElementAtOrDefault(11).Value?.ToString().Split(",")[0] ?? string.Empty;
            string currentHeading = null;
            foreach (var row in rows)
            {
                var rowdata = (IDictionary<string, object>)row;

                var mapCode = rowdata.ElementAtOrDefault(0).Value;
                var Description = rowdata.ElementAtOrDefault(1).Value;
                var headingName = rowdata.ElementAtOrDefault(1).Value;
                var RevMonth = rowdata.ElementAtOrDefault(19).Value;
                
                if (mapCode == null || Description == null)
                {
                    continue;
                }
                if (RevMonth == null)
                {
                    
                    if (!int.TryParse(mapCode.ToString(), out int mapCodes))
                    {
                        continue;
                    }
                    currentHeading = headingName.ToString();
                    var excelFileForData = new ExcelsFiles
                    {
                        HotelName = hotelName,
                        HeadingName = currentHeading.ToString(),
                       
                    };
                    dataList.Add(excelFileForData);
                }
                else
                {
                    if (!int.TryParse(mapCode.ToString(), out int mapCodes) ||
                        !decimal.TryParse(RevMonth.ToString(), out decimal revmonth))
                    {
                        continue;
                    }

                    var excelFileForData = new ExcelsFiles
                    {
                        HotelName = hotelName,
                        MappingCode = mapCodes,
                        Description = Description.ToString(),
                        HeadingName = currentHeading.ToString(),
                        RevMonth = revmonth,
                    };
                    dataList.Add(excelFileForData);
                }

            }

            return dataList;
        }
    }
}
