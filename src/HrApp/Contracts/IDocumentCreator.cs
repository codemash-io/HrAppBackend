using HrApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp.Contracts
{
    public interface IDocumentCreator
    {
        Task GenerateAbsenceWordAsync(EmployeeEntity employee, string reason, AbsenceRequestEntity absenceRequest,
                                        string end, string absenceType, string days);
        void GenerateEmployeeReportWord(EmployeeEntity employee, DateTime startDate, DateTime endDate);
        void GenerateProjectsReportWord(List<ProjectEntity> projects, DateTime startDate, DateTime endDate);
        void GenerateSelectedProjectReportWord(ProjectEntity project, DateTime startDate, DateTime endDate);
        void GenerateEmployeeReportExcel(EmployeeEntity employee, DateTime dateFrom, DateTime dateTo);
        void GenerateProjectReportExcel(ProjectEntity project, DateTime dateFrom, DateTime dateTo);
        void GenerateMultipleProjectsReportExcel(List<ProjectEntity> projects, DateTime dateFrom, DateTime dateTo);
    }
}
