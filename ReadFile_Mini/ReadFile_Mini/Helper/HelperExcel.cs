using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MiniExcelLibs;
using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Models;


namespace ReadFile_Mini.Helper
{
    public class HelperExcel
    {
        private readonly SeniorDb _seniorDb;
        public HelperExcel(SeniorDb seniorDb)
        {
            _seniorDb = seniorDb;
        }
        public async Task<List<HouseState>> ReadExcel(Stream fileStream)
        {
            List<HouseState> dataList = new List<HouseState>();

            var rows = MiniExcel.Query(fileStream).ToList();
            if (rows == null || rows.Count == 0)
            {
                throw new Exception("No worksheet found in the Excel file or worksheet is empty.");
            }

            var firstRow = (IDictionary<string, object>)rows[0];
            string hotelName = firstRow.ElementAtOrDefault(16).Value?.ToString().Split(",")[0] ?? string.Empty;

            foreach (var row in rows)
            {
                try
                {
                    var rowData = (IDictionary<string, object>)row;

                    var dateCell = rowData.ElementAtOrDefault(0).Value;
                    var freeCell = rowData.ElementAtOrDefault(2).Value;
                    var occupiedCell = rowData.ElementAtOrDefault(6).Value;
                    var accommodationCell = rowData.ElementAtOrDefault(37).Value;
                    var FBCell = rowData.ElementAtOrDefault(38).Value;
                    var extCell = rowData.ElementAtOrDefault(41).Value;
                    var totalCell = rowData.ElementAtOrDefault(43).Value;
                    var FreePercentage = rowData.ElementAtOrDefault(5).Value;
                    var OccupiedPercentage = rowData.ElementAtOrDefault(9).Value;
                    var BedsPercentage = rowData.ElementAtOrDefault(13).Value;
                    var ArrivalsNo = rowData.ElementAtOrDefault(20).Value;
                    var ArrivalsPers = rowData.ElementAtOrDefault(21).Value;
                    var DeparturesNo = rowData.ElementAtOrDefault(27).Value;
                    var DeparturesPers = rowData.ElementAtOrDefault(28).Value;
                    var InHouseNo = rowData.ElementAtOrDefault(32).Value;
                    var InHousePers = rowData.ElementAtOrDefault(33).Value;
                    var Acc = rowData.ElementAtOrDefault(37).Value;
                    var ADR = rowData.ElementAtOrDefault(47).Value;
                    var RevPar = rowData.ElementAtOrDefault(48).Value;

                    if (dateCell == null || freeCell == null || occupiedCell == null || accommodationCell == null || FBCell == null
                        || extCell == null || totalCell == null || FreePercentage == null || OccupiedPercentage == null ||
                       BedsPercentage == null || ArrivalsNo == null || ArrivalsPers == null || DeparturesNo == null
                       || DeparturesPers == null || InHouseNo == null || InHousePers == null || Acc == null || ADR == null ||
                       RevPar == null)
                    {
                        continue;
                    }

                    string[] dateFormats = { "MM/dd/yyyy", "dd/MM/yyyy", "ddd, dd.MM.yyyy" };
                    if (!DateTime.TryParseExact(dateCell.ToString(), dateFormats, null, DateTimeStyles.None, out DateTime date))
                    {
                        continue;
                    }

                    if (!int.TryParse(freeCell.ToString(), out int free) ||
                        !int.TryParse(occupiedCell.ToString(), out int occupied) ||
                        !decimal.TryParse(accommodationCell.ToString(), out decimal accommodation) ||
                        !decimal.TryParse(FBCell.ToString(), out decimal FB) ||
                        !decimal.TryParse(extCell.ToString(), out decimal extra) ||
                        !decimal.TryParse(totalCell.ToString(), out decimal total) ||
                        !double.TryParse(FreePercentage.ToString(), out double freePer) ||
                        !double.TryParse(OccupiedPercentage.ToString(), out double OccupiedPer) ||
                        !double.TryParse(BedsPercentage.ToString(), out double BedsPer) ||
                        !int.TryParse(ArrivalsNo.ToString(), out int ArrNo) ||
                        !int.TryParse(ArrivalsPers.ToString(), out int ArrsPers) ||
                        !int.TryParse(DeparturesNo.ToString(), out int DeparturesNos) ||
                        !int.TryParse(DeparturesPers.ToString(), out int DeparturesPerss) ||
                        !int.TryParse(InHouseNo.ToString(), out int InHouseNos) ||
                        !int.TryParse(InHousePers.ToString(), out int InHousePerss) ||
                        !decimal.TryParse(Acc.ToString(), out decimal Accs) ||
                        !double.TryParse(ADR.ToString(), out double ADRs) ||
                        !double.TryParse(RevPar.ToString(), out double RevPars))
                    {
                        continue;
                    }

                    var houseState = new HouseState
                    {
                        HouseDate = date.ToString("ddd, dd.MM.yyyy"),
                        Free = free,
                        occupied = occupied,
                        Accommodation = Math.Round(accommodation, 2),
                        FB = Math.Round(FB, 2),
                        Extras = Math.Round(extra, 2),
                        Total = Math.Round(total, 2),
                        HotelName = hotelName,
                        FreePercentage = Math.Round(freePer, 2),
                        OccupiedPercentage = Math.Round(OccupiedPer, 2),
                        BedsPercentage = Math.Round(BedsPer, 2),
                        ArrivalsNo = ArrNo,
                        ArrivalsPers = ArrsPers,
                        DeparturesNo = ArrsPers,
                        DeparturesPers = ArrsPers,
                        InHouseNo = InHouseNos,
                        InHousePers = InHousePerss,
                        Acc = Math.Round(Accs, 2),
                        ADR = Math.Round(ADRs, 2),
                        RevPar = Math.Round(RevPars, 2),


                    };

                    dataList.Add(houseState);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing row: {ex.Message}");
                    continue;
                }
            }

            await _seniorDb.HouseStates.AddRangeAsync(dataList);
            await _seniorDb.SaveChangesAsync();

            return dataList;
        }

    }
}
