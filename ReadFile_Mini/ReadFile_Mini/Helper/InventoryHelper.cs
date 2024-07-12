using MiniExcelLibs;
using ReadFile_Mini.Context;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Helper
{
    public class InventoryHelper
    {
        private readonly SeniorDb _Db;
        public InventoryHelper(SeniorDb Db)
        {
            _Db = Db;
        }

        public async Task<List<Inventory>> ReadExcel(Stream fileStream)
        {
            List<Inventory> DataList = new List<Inventory>();

            var rows = MiniExcel.Query(fileStream).ToList();

            if (rows == null || rows.Count == 0)
            {
                throw new Exception("No worksheet found in the Excel file or worksheet is empty.");
            }


            foreach (var row in rows)
            {
                var rowdata = (IDictionary<string, object>)row;

                var InventoryId = rowdata.ElementAtOrDefault(0).Value;
                var ParentId = rowdata.ElementAtOrDefault(1).Value;
                var InventoryCode = rowdata.ElementAtOrDefault(2).Value;
                var StartInv = rowdata.ElementAtOrDefault(3).Value;
                var EndInv = rowdata.ElementAtOrDefault(4).Value;
                var InvTypeCode = rowdata.ElementAtOrDefault(5).Value;
                var IsRoom = rowdata.ElementAtOrDefault(6).Value;

                // Check if all properties are null
                if (InventoryId == null && ParentId == null && InventoryCode == null &&
                    StartInv == null && EndInv == null && InvTypeCode == null && IsRoom == null)
                {
                    continue;
                }

                if (!int.TryParse(InventoryId.ToString(), out int InventoryIds) ||
                !int.TryParse(IsRoom.ToString(), out int IsRooms))
                {
                    continue;
                }


                var InventoryByDate = new Inventory
                {
                    InventoryId = InventoryIds,
                    ParentId = ParentId.ToString() ?? "null",
                    InventoryCode = InventoryCode.ToString() ?? "null",
                    StartInv =StartInv.ToString() ?? "null",
                    EndInv = EndInv.ToString() ?? "null",
                    InvTypeCode = InvTypeCode.ToString() ?? "null",
                    IsRoom = IsRooms,
                };

                DataList.Add(InventoryByDate);
            }

            await _Db.Inventory.AddRangeAsync(DataList);
            await _Db.SaveChangesAsync();

            return DataList;
        }
    }
}
