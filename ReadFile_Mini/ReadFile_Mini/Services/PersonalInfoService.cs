using Microsoft.EntityFrameworkCore;
using ReadFile_Mini.Context;
using ReadFile_Mini.Interface;
using ReadFile_Mini.Models;
using ReadFile_Mini.Requestes;
using ReadFile_Mini.Response;
using System.Runtime.CompilerServices;

namespace ReadFile_Mini.Services
{
    public class PersonalInfoService : IpersonalInfoServicecs
    {
        private readonly SeniorDb _db;
        private readonly IpersonalInfoRepository _personalInfoRepository;
        private  readonly IGenericRepository<personalRequest> _repository;
       
        public PersonalInfoService(SeniorDb db, IpersonalInfoRepository personalInfoRepository , IGenericRepository<personalRequest> repository)
        {
            _db = db;
            _personalInfoRepository = personalInfoRepository;
            _repository = repository;
        }

        public async Task<bool> AddPersonalAsync(personalRequest personalRequest, List<string> languages)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingPersonalInfo = await _personalInfoRepository.GetByEmailAsync(personalRequest.Email);

                    if (existingPersonalInfo != null)
                    {
                        return false;
                    }

                    var personalInfo = new PersonalInfo
                    {
                        FirstName = personalRequest.FirstName,
                        LastName = personalRequest.LastName,
                        Email = personalRequest.Email,
                        MobileNo = personalRequest.MobileNo,
                        Gender = personalRequest.Gender,
                        MaritalStatus = personalRequest.MaritalStatus,
                        Address = personalRequest.Address,
                        City = personalRequest.City,
                        State = personalRequest.State,
                        Zip = personalRequest.Zip,
                        Race = personalRequest.Race,
                        Languages = string.Join(", ", personalRequest.Languages),
                        CreatedDate = DateTime.Now,
                    };

                    await _personalInfoRepository.AddPersonalInfoAsync(personalInfo);
                    await _personalInfoRepository.SaveChangesAsync();

                    foreach (var languageName in languages)
                    {
                        var language = await _personalInfoRepository.GetLanguageByNameAsync(languageName);
                        if (language != null)
                        {
                            var personLanguage = new PersonLanguage
                            {
                                PersonalInfoId = personalInfo.Id,
                                LanguageId = language.Id,
                            };

                            await _personalInfoRepository.AddPersonLanguageAsync(personLanguage);
                        }
                    }

                    await _personalInfoRepository.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
        public async Task<bool> DeletePersonalAsync(int id)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    await _personalInfoRepository.DeletePersonalAsync(id);
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<IEnumerable<PersonalInfo>> GetAllPersonalInfoAsync()
        {
            return await _personalInfoRepository.GetAllPersonalInfoAsync();
        }

        public async Task<PersonalInfo> GetPersonalInfoByIdAsync(int id)
        {
            return await _personalInfoRepository.GetPersonalInfoByIdAsync(id);
        }

        public async Task<bool> UpdatePersonalAsync(int id, personalRequest updatedPersonalRequest)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingPersonalInfo = await _personalInfoRepository.GetPersonalInfoByIdAsync(id);
                    if (existingPersonalInfo == null)
                    {
                        throw new KeyNotFoundException("Personal info not found");
                    }

                    existingPersonalInfo.FirstName = updatedPersonalRequest.FirstName;
                    existingPersonalInfo.LastName = updatedPersonalRequest.LastName;
                    existingPersonalInfo.Email = updatedPersonalRequest.Email;
                    existingPersonalInfo.MobileNo = updatedPersonalRequest.MobileNo;
                    existingPersonalInfo.Gender = updatedPersonalRequest.Gender;
                    existingPersonalInfo.MaritalStatus = updatedPersonalRequest.MaritalStatus;
                    existingPersonalInfo.Address = updatedPersonalRequest.Address;
                    existingPersonalInfo.City = updatedPersonalRequest.City;
                    existingPersonalInfo.State = updatedPersonalRequest.State;
                    existingPersonalInfo.Zip = updatedPersonalRequest.Zip;
                    existingPersonalInfo.Race = updatedPersonalRequest.Race;
                    existingPersonalInfo.Languages = string.Join(", ", updatedPersonalRequest.Languages);

                    var result = await _personalInfoRepository.UpdatePersonalAsync(existingPersonalInfo, updatedPersonalRequest.Languages);

                    await transaction.CommitAsync();

                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

    }
}
