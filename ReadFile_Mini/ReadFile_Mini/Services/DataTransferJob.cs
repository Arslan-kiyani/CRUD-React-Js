using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Services
{
    public class DataTransferJob
    {
        private readonly SeniorDb _sqlServerDbContext;
        private readonly PostgreSqlDbContext _postgreSqlDbContext;
        private readonly ILogger<DataTransferJob> _logger; 

        public DataTransferJob(SeniorDb sqlServerDbContext, PostgreSqlDbContext postgreSqlDbContext, ILogger<DataTransferJob> logger)
        {
            _postgreSqlDbContext = postgreSqlDbContext;
            _sqlServerDbContext = sqlServerDbContext;
            _logger = logger;
        }

        public async Task TransferData()
        {
            try
            {
                var dataToTransfer = await _sqlServerDbContext.Inventory.ToListAsync();

                foreach (var d in dataToTransfer)
                {
                    var transformedData = new InventoryDestination
                    {
                        inventoryid = d.InventoryId,
                        parentid = d.ParentId,
                        inventorycode = d.InventoryCode,
                        invtypecode = d.InvTypeCode,
                        isroom = d.IsRoom,
                        startinv = d.StartInv,
                        endinv = d.EndInv,

                    };
                    var existingData = await _postgreSqlDbContext.inventorydestinations.FirstOrDefaultAsync(u =>
                        u.inventoryid == transformedData.inventoryid &&
                        u.parentid == transformedData.parentid && 
                        u.inventorycode == transformedData.inventorycode &&
                        u.invtypecode == transformedData.invtypecode &&
                        u.isroom == transformedData.isroom &&
                        u.startinv == transformedData.startinv && 
                        u.endinv == transformedData.endinv);

                    if (existingData == null) 
                    {
                         await _postgreSqlDbContext.inventorydestinations.AddAsync(transformedData);
                        _logger.LogInformation($"Data transferred: {DateTime.Now}");
                    }
                    else
                    {
                        _logger.LogInformation($"Skipping data transfer for InventoryId: {transformedData.inventoryid} - Already exists in PostgreSqlDb.");
                    }
                }
                
                 await _postgreSqlDbContext.SaveChangesAsync();
                _logger.LogInformation("Data transfer completed successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error transferring data: {ex.Message}");
                
            }
        }
    }
}
