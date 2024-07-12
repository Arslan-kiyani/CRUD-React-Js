using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using MiniExcelLibs;
using ReadFile_Mini.Context;
using ReadFile_Mini.Models;
using System.Globalization;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReadFile_Mini.Helper
{
    public class JournalHelper
    {
        private readonly SeniorDb _seniorDb;
        public JournalHelper( SeniorDb seniorDb)
        {
            _seniorDb = seniorDb;
        }

        public async Task<List<JournalByDate>> ReadExcel(Stream fileStream)
        {
            List<JournalByDate> dataList = new List<JournalByDate>();

            var rows = MiniExcel.Query(fileStream).ToList();
            if (rows == null || rows.Count == 0)
            {
                throw new Exception("No worksheet found in the Excel file or worksheet is empty.");
            }

            var firstRow = (IDictionary<string, object>)rows[0];
            string hotelName = firstRow.ElementAtOrDefault(19).Value?.ToString().Split(",")[0] ?? string.Empty;

            foreach ( var row in rows )
            {
                var rowdata = (IDictionary<string, object>)row;

                var InvoiceOne = rowdata.ElementAtOrDefault(0).Value;
                var InvoiceTwo = rowdata.ElementAtOrDefault(1).Value;
                var res = rowdata.ElementAtOrDefault(10).Value;
                var Quantity = rowdata.ElementAtOrDefault(13).Value;
                var InvoiceText = rowdata.ElementAtOrDefault(18).Value;
                var Single =  rowdata.ElementAtOrDefault(20).Value;
                var total = rowdata.ElementAtOrDefault(23).Value;
                var user = rowdata.ElementAtOrDefault(26).Value;
                var room = rowdata.ElementAtOrDefault(27).Value;
                var cat = rowdata.ElementAtOrDefault(30).Value;
                var guess = rowdata.ElementAtOrDefault(31).Value;
                var Arrival = rowdata.ElementAtOrDefault(34).Value;
                var Depart = rowdata.ElementAtOrDefault(35).Value;
               
                if (InvoiceOne != null  && InvoiceOne.ToString().Any(char.IsLetter))
                {
                    continue;
                }
                if (room == null || cat == null || Arrival == null || Depart == null || res == null || InvoiceTwo == null)
                {
                    room = 0; cat = ""; Arrival=""; Depart=""; res = ""; InvoiceTwo = "";
                }
                
                if (Quantity == null ||
                    Single == null || total == null || user == null )
                {
                    continue; 
                }
               

                if ( !decimal.TryParse(Single.ToString(), out decimal Singles) || 
                !decimal.TryParse(total.ToString(), out decimal totals) || 
                !int.TryParse(room.ToString(), out int rooms) || 
                !int.TryParse(Quantity.ToString(), out int quantity))
                {
                    continue;
                }

                // Convert Arrival and Depart to DateTime
                string formattedArrivalDate = ConvertDecimalToDate(Arrival);
                string formattedDepartDate = ConvertDecimalToDate(Depart);

                var JournalByDate = new JournalByDate
                {
                    InvoiceOne = InvoiceOne.ToString() ?? "null",
                    HotelName = hotelName,
                    InvoiceText = InvoiceText.ToString() ?? "null",
                    Category = cat.ToString() ?? "null",
                    User = user.ToString() ?? "null",
                    Guest = guess.ToString()?? "null",
                    Reservation = res.ToString() ?? "null",
                    Single = Singles,
                    Total = totals,
                    Room = rooms,
                    Quantity = quantity,
                    Arrival = formattedArrivalDate ?? "",
                    Departure = formattedDepartDate ?? "",
                    InvoiceTwo = InvoiceTwo.ToString() ?? "null",

                };

                // Check if the entry already exists in the database
                bool exists = await _seniorDb.journalByDates.AnyAsync(j => j.InvoiceOne == JournalByDate.InvoiceOne &&
                                                                           j.HotelName == JournalByDate.HotelName &&
                                                                           j.InvoiceText == JournalByDate.InvoiceText &&
                                                                           j.Category == JournalByDate.Category &&
                                                                           j.User == JournalByDate.User &&
                                                                           j.Guest == JournalByDate.Guest &&
                                                                           j.Reservation == JournalByDate.Reservation &&
                                                                           j.Single == JournalByDate.Single &&
                                                                           j.Total == JournalByDate.Total &&
                                                                           j.Room == JournalByDate.Room &&
                                                                           j.Quantity == JournalByDate.Quantity &&
                                                                           j.Arrival == JournalByDate.Arrival &&
                                                                           j.Departure == JournalByDate.Departure &&
                                                                           j.InvoiceTwo == JournalByDate.InvoiceTwo);
                if (!exists)
                {
                    dataList.Add(JournalByDate);
                }
            }

            if (dataList.Any())
            {
                await _seniorDb.journalByDates.AddRangeAsync(dataList);
                await _seniorDb.SaveChangesAsync();
            }

            return dataList;

        }
        private string ConvertDecimalToDate(object decimalDateObj)
        {
            
            if (decimalDateObj is DateTime dateTime)
            {
                return dateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            if (decimalDateObj == null || !decimal.TryParse(decimalDateObj.ToString(), out decimal decimalDate))
            {
                return "Invalid date";
            }

            DateTime epoch = new DateTime(1900, 1, 1);
            DateTime date = epoch.AddDays((double)decimalDate - 2);

            // Format the date as dd.MM.yyyy
            return date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }
    }
}
