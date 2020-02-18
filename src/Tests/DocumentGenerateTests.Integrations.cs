using System;
using System.Threading.Tasks;
using HrApp;
using HrApp.Generators;
using HrApp.Repositories;
using Xunit;

namespace Tests
{
    public class DocumentGenerateTests
    {
        [Fact]
        public async Task Test_Generate_Vacation_Word()
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
                Address = "Pašiles 37",
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

            await repo.GenerateAbsenceWordAsync(employee, " Kasmetiniu atostogu", abscence, "a", "a", "a");
        }
        [Fact]
        public async Task Test_Check_If_Trigger_Works()
        {
            var repo = new AbsenceRepository();
            await repo.UpdateStatus();

        }

        [Fact]
        public async Task Test_Generate_Employee_Report_Word()
        {
            var repo = new WordCreator()
            {
                FileRepo = new FileRepository(),
            };
            var empRepo = new EmployeesRepository();
            var employee = await empRepo.GetEmployeeByLastName("t");

            repo.GenerateEmploeeReportWord(employee, new DateTime(2020, 02, 03), new DateTime(2020, 02, 09));
        }

        [Fact]
        public async Task Test_Generate_Project_Report_Word()
        {
            var repo = new WordCreator()
            {
                FileRepo = new FileRepository(),
            };
            var proRepo = new ProjectRepository();
            var project = await proRepo.GetProjectByName("pro2");

            repo.GenerateSelectedProjectReportWord(project, new DateTime(2020, 01, 01), new DateTime(2020, 01, 31));
        }

        [Fact]
        public async Task Test_Generate_Multiple_Projects_Report_Word()
        {
            var repo = new WordCreator()
            {
                FileRepo = new FileRepository(),
            };
            var proRepo = new ProjectRepository();
            var projects = await proRepo.GetAllProjects();

            repo.GenerateProjectsReportWord(projects, new DateTime(2020, 01, 01), new DateTime(2020, 01, 31));
        }

        [Fact]
        public async Task Test_Generate_Employee_Report_Excel()
        {
            var repo = new ExcelCreator()
            {
                FileRepo = new FileRepository(),
            };
            var empRepo = new EmployeesRepository();
            var employee = await empRepo.GetEmployeeByLastName("t");

            repo.GenerateEmployeeReportExcel(employee, new DateTime(2020, 02, 03), new DateTime(2020, 02, 09));
        }

        [Fact]
        public async Task Test_Generate_Project_Report_Excel()
        {
            var repo = new ExcelCreator()
            {
                FileRepo = new FileRepository(),
            };

            var proRepo = new ProjectRepository();
            var project = await proRepo.GetProjectByName("pro2");

            repo.GenerateProjectReportExcel(project, new DateTime(2020, 02, 03), new DateTime(2020, 02, 09));
        }

        [Fact]
        public async Task Test_Generate_Multiple_Projects_Report_Excel()
        {
            var repo = new ExcelCreator()
            {
                FileRepo = new FileRepository(),
            };

            var proRepo = new ProjectRepository();
            var projects = await proRepo.GetAllProjects();

            repo.GenerateMultipleProjectsReportExcel(projects, new DateTime(2020, 01, 01), new DateTime(2020, 01, 31));
        }

    }
}
