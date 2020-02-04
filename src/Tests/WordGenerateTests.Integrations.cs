using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrApp;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class WordGenerateTests
    {
             
        [Fact]

        public async Task Test_Generate_Word()
        {
            var repo = new WordCreator()
            {
                FileRepo = new FileRepository(),
            };
            EmployeeEntity employee = new EmployeeEntity()
            {
                FirstName = "Aurimas",
                LastName = "Valauskas",
                Division = "Kaunas",
                Address = "Pašilės 37",
                Id = "5e1dcf60d1930300012f1106"

            };
            AbsenceRequestEntity abscence = new AbsenceRequestEntity()
            {
                AbsenceStart = new DateTime(2020, 02, 02),
                AbsenceEnd = new DateTime(2020, 02, 02),
                Id = "5e32c947f06da80001a12f4a"
            };
            //var DateFrom = new DateTime(2020, 01, 28);
            //   var DateTo = new DateTime(2020, 02, 25);

            await repo.GenerateWordAsync(employee, " Kasmetinių atostogų", abscence, "a","a", "a");
        }
        [Fact]
        public async Task Test_Check_If_Trigger_Works()
        {
            var repo = new AbsenceRepository();
            repo.UpdateStatus();

        }

    }
}
