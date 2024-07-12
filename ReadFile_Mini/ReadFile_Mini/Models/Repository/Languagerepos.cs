using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Repository
{
    public class Languagerepos : ILanguagerepos
    {
        private readonly SeniorDb _seniorDb;
        public Languagerepos(SeniorDb seniorDb)
        {
            _seniorDb = seniorDb; 
        }
        public async Task<IEnumerable<Language>> GetAllLanguageAsync()
        {
            // Use Entity Framework Core to query the database and retrieve all languages asynchronously
            return await _seniorDb.Languages.ToListAsync();
        }
    }
}
