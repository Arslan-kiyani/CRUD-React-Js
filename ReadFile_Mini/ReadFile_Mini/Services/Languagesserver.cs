using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;
using ReadFile_Mini.Repository;

namespace ReadFile_Mini.Services
{
    public class Languagesserver : ILanguagesserver
    {
        private readonly ILanguagerepos _languagerepos;
        public Languagesserver(ILanguagerepos languagerepos)
        {
            _languagerepos = languagerepos;
        }
        public async Task<IEnumerable<Language>> GetAllLanguageAsync()
        {
            return await _languagerepos.GetAllLanguageAsync();
        }
    }
}
