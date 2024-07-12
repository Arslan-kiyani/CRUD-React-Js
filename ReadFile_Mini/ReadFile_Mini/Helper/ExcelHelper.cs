using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using ReadFile_Mini.Context;
using ReadFile_Mini.Models;
using System.Globalization;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReadFile_Mini.Helper
{
    public class ExcelHelper
    {
        private readonly SeniorDb _eniorDb;
        public ExcelHelper(SeniorDb eniorDb)
        {
            _eniorDb = eniorDb;
        }
        public async Task<List<HouseState>> ReadExcel(Stream fileStream)
        {
            List<HouseState> dataList = new List<HouseState>();

            using (var package = new ExcelPackage(fileStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                if (worksheet == null)
                {
                    throw new Exception("No worksheet found in the Excel file.");
                }

                int rowCount = worksheet.Dimension?.Rows ?? 0;
                if (rowCount == 0)
                {
                    throw new Exception("Worksheet is empty.");
                }
                var hotelName = worksheet.Cells[1, 17].Value;
                if (hotelName != null)
                {
                    hotelName = hotelName.ToString().Split(",")[0];
                }
                for (int row = 1; row <= rowCount; row++)
                {
                    try
                    {

                        var dateCell = worksheet.Cells[row, 1].Value;
                        var free = worksheet.Cells[row, 3].Value;
                        var occupied = worksheet.Cells[row, 7].Value;
                        var accommodation = worksheet.Cells[row, 38].Value;
                        var FB = worksheet.Cells[row, 39].Value;
                        var ext = worksheet.Cells[row, 42].Value;
                        var total = worksheet.Cells[row, 44].Value;


                        if (dateCell == null || free == null || occupied == null || accommodation == null || FB == null || ext == null ||
                            total == null)
                        {
                            continue;
                        }

                        string[] dateFormats = { "MM/dd/yyyy", "dd/MM/yyyy", "ddd, dd.MM.yyyy" };
                        if (!DateTime.TryParseExact(dateCell.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out DateTime date))
                        {
                            continue;
                        }
                        var housedate = dateCell.ToString();

                        if (!int.TryParse(free.ToString(), out int freePercentage1) ||
                            !int.TryParse(occupied.ToString(), out int occupiedPercentage1) ||
                            !decimal.TryParse(accommodation.ToString(), out decimal accommodations) ||
                            !decimal.TryParse(FB.ToString(), out decimal FBs) ||
                            !decimal.TryParse(ext.ToString(), out decimal extra) ||
                            !decimal.TryParse(total.ToString(), out decimal totals))
                        {
                            continue;
                        }

                        accommodations = Math.Round(accommodations, 2);
                        FBs = Math.Round(FBs, 2);
                        extra = Math.Round(extra, 2);
                        totals = Math.Round(totals, 2);

                        HouseState data = new HouseState
                        {
                            HouseDate = housedate,
                            Free = freePercentage1,
                            occupied = occupiedPercentage1,
                            Accommodation = accommodations,
                            FB = FBs,
                            Extras = extra,
                            Total = totals,
                            HotelName = hotelName.ToString(),
                        };

                        dataList.Add(data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing row {row}: {ex.Message}");
                        continue;
                    }
                }
                await _eniorDb.HouseStates.AddRangeAsync(dataList);
                _eniorDb.SaveChanges();
                return dataList;
            }
        }
    }
}
