using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Context
{
    public class SeniorDb : DbContext
    {
        public SeniorDb(DbContextOptions<SeniorDb> options) : base(options)
        {
        }

        public DbSet<UserTable> UserTable { get; set; }
        public DbSet<Trip> Trip { get; set; }
        public DbSet<UserTrip> UserTrip { get; set; }
        public DbSet<HouseState> HouseStates { get; set; }
        public DbSet<JournalByDate> journalByDates { get; set; }
        public DbSet<sdva> sdvas { get; set; }
        public DbSet<ExcelsFiles> ExcelsFiles { get; set; }



        public DbSet<Race> Race { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<PersonLanguage> PersonLanguages { get; set; }

        public DbSet<SourceData> SourceData { get; set; }
        public DbSet<Inventory> Inventory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your model here if needed
            modelBuilder.Entity<Trip>()
                .HasKey(t => t.TripId); // Define the primary key
        }
    }
}
