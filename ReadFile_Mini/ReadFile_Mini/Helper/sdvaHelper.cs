using MiniExcelLibs;
using ReadFile_Mini.Context;
using ReadFile_Mini.Models;

namespace ReadFile_Mini.Helper
{
    public class sdvaHelper
    {
        private readonly SeniorDb _eniorDb;
        public sdvaHelper(SeniorDb eniorDb)
        {
            _eniorDb = eniorDb;
        }
        public async Task<List<sdva>> ReadExcel(Stream fileStream)
        {
            List<sdva> allDataList = new List<sdva>();

            var rows = MiniExcel.Query(fileStream).ToList();
            if (rows == null || rows.Count == 0)
            {
                throw new Exception("No worksheet found in the Excel file or worksheet is empty.");
            }

            List<sdva> currentBatch = new List<sdva>();
            const int batchSize = 100;

            foreach (var row in rows)
            {
                var rowdata = (IDictionary<string, object>)row;

                var EDI = rowdata.ElementAtOrDefault(0).Value;
                var FName = rowdata.ElementAtOrDefault(2).Value;
                var LName = rowdata.ElementAtOrDefault(1).Value;
               // var MiddleName = rowdata.ElementAtOrDefault(3).Value;
                var Department = rowdata.ElementAtOrDefault(4).Value;
                var ssn = rowdata.ElementAtOrDefault(5).Value;
                //var DATE_OF_ENTRY = rowdata.ElementAtOrDefault(15).Value;
                //var DATEOFSEPARATION = rowdata.ElementAtOrDefault(16).Value;
                var EMAIL_ADDRESS = rowdata.ElementAtOrDefault(22).Value;
                //var MemberR = rowdata.ElementAtOrDefault(30).Value;
                //var GENDER  = rowdata.ElementAtOrDefault(4).Value;
                //var RANK = rowdata.ElementAtOrDefault(7).Value;
                //var PAY_GRADE = rowdata.ElementAtOrDefault(8).Value;
                //var DOB = rowdata.ElementAtOrDefault(9).Value;
                //var PLACE_OF_ENTRY = rowdata.ElementAtOrDefault(10).Value;
                //var HOME_OF_REC_ATE = rowdata.ElementAtOrDefault(11).Value;
                //var LAST_DUTY_ASSIGNMENT = rowdata.ElementAtOrDefault(12).Value;
                //var STATION_WHERE_SEPARATED = rowdata.ElementAtOrDefault(13).Value;
                //var PRIMARY_SPECIALTIES = rowdata.ElementAtOrDefault(14).Value;
                //var NET_ACTIVE_SERVICES_YR_MN_DY = rowdata.ElementAtOrDefault(17).Value;
                //var TOT_PRIOR_ACT_SERVICE_YR_MN_DY = rowdata.ElementAtOrDefault(18).Value;
                //var MEDALS_AND_AWARDS = rowdata.ElementAtOrDefault(19).Value;
                //var REMARKS = rowdata.ElementAtOrDefault(20).Value;
                //var MILITARY_EDUCATION = rowdata.ElementAtOrDefault(21).Value;
                var PHONE_NUMBER = rowdata.ElementAtOrDefault(23).Value;
                //var MAIL_ADDRESS_AFTER_SEPARATION = rowdata.ElementAtOrDefault(24).Value;
                //var LATEST_MAILING_ADDRESS_LINE1 = rowdata.ElementAtOrDefault(25).Value;
                //var LINE2 = rowdata.ElementAtOrDefault(26).Value;
                //var CITY = rowdata.ElementAtOrDefault(27).Value;
                //var STATE = rowdata.ElementAtOrDefault(28).Value;
                //var ZIP = rowdata.ElementAtOrDefault(29).Value;
                //var MEMBER_REQ_COPY_3 = rowdata.ElementAtOrDefault(30).Value;
                //var TYPE_OF_SEPARATION = rowdata.ElementAtOrDefault(31).Value;
                //var CHARACTER_OF_SERVICE = rowdata.ElementAtOrDefault(32).Value;
                //var NARRATIVE_FOR_SEPARATION = rowdata.ElementAtOrDefault(33).Value;
                //var TIME_LOST = rowdata.ElementAtOrDefault(34).Value;


                if (EDI == null || FName == null || LName == null || 
                    Department == null || ssn == null || PHONE_NUMBER ==null || EMAIL_ADDRESS ==null)
                {
                    continue;
                   
                }

                var sdvaByDate = new sdva
                {
                    EDI = EDI.ToString(),
                    FName = FName.ToString(),
                    LName = LName.ToString(),
                    Department = Department.ToString(),
                    SSN = ssn.ToString(),
                    PHONE_NUMBER = PHONE_NUMBER.ToString(),
                    EMAIL_ADDRESS = EMAIL_ADDRESS.ToString(),

                };

                currentBatch.Add(sdvaByDate);
                allDataList.Add(sdvaByDate);

                if (currentBatch.Count == batchSize)
                {
                    await _eniorDb.sdvas.AddRangeAsync(currentBatch);
                    await _eniorDb.SaveChangesAsync();
                    currentBatch.Clear();
                }
            }

            if (currentBatch.Count > 0)
            {
                await _eniorDb.sdvas.AddRangeAsync(currentBatch);
                await _eniorDb.SaveChangesAsync();
            }

            return allDataList;
        }

    }
}
