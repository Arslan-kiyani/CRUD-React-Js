using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;

namespace ReadFile_Mini.Repository
{
    public class personalInfoRepository : IpersonalInfoRepository
    {

        private readonly SeniorDb _seniorDb;
        public personalInfoRepository(SeniorDb seniorDb)
        {
            _seniorDb = seniorDb;
        }

        public async Task<IEnumerable<PersonalInfo>> GetAllPersonalInfoAsync()
        {
            return await _seniorDb.PersonalInfo
                          .OrderByDescending(pi => pi.CreatedDate)
                          .ToListAsync();
        }

        public async Task<PersonalInfo> GetPersonalInfoByIdAsync(int id)
        {
            return await _seniorDb.PersonalInfo.FindAsync(id);
        }

        public async Task<bool> DeletePersonalAsync(int id)
        {
            var personalInfo = await _seniorDb.PersonalInfo.FindAsync(id);
            if (personalInfo == null)
            {
                throw new KeyNotFoundException("Personal info not found");
            }

            // Remove related records from PersonLanguages table
            var relatedLanguages = await _seniorDb.PersonLanguages.Where(pl => pl.PersonalInfoId == id).ToListAsync();
            _seniorDb.PersonLanguages.RemoveRange(relatedLanguages);


            _seniorDb.PersonalInfo.Remove(personalInfo);
            await _seniorDb.SaveChangesAsync();
            return false;
        }

        public async Task<bool> UpdatePersonalAsync(PersonalInfo personalInfo, List<string> languages)
        {
            try
            {
                _seniorDb.PersonalInfo.Update(personalInfo);

                // Optionally: Update the languages associated with the person
                var existingLanguages = _seniorDb.PersonLanguages.Where(pl => pl.PersonalInfoId == personalInfo.Id);
                _seniorDb.PersonLanguages.RemoveRange(existingLanguages);

                // Add new language associations
                foreach (var languageName in languages)
                {
                    var language = await _seniorDb.Languages.SingleOrDefaultAsync(l => l.Name == languageName);
                    if (language != null)
                    {
                        var personLanguage = new PersonLanguage
                        {
                            PersonalInfoId = personalInfo.Id,
                            LanguageId = language.Id,
                        };
                        await _seniorDb.PersonLanguages.AddAsync(personLanguage);
                    }
                }

                await _seniorDb.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public async Task<PersonalInfo> GetByEmailAsync(string email)
        {
            return await _seniorDb.PersonalInfo.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<Language> GetLanguageByNameAsync(string name)
        {
            return await _seniorDb.Languages.FirstOrDefaultAsync(l => l.Name == name);
        }

        public async Task AddPersonalInfoAsync(PersonalInfo personalInfo)
        {
            await _seniorDb.PersonalInfo.AddAsync(personalInfo);
        }

        public async Task AddPersonLanguageAsync(PersonLanguage personLanguage)
        {
            await _seniorDb.PersonLanguages.AddAsync(personLanguage);
        }

        public async Task SaveChangesAsync()
        {
            await _seniorDb.SaveChangesAsync();
        }

        
    }
}
