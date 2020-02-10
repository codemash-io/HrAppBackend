using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemBox.Spreadsheet;
using HrApp.Contracts;
using HrApp.Entities;
using HrApp.Repositories;

namespace HrApp.Generators
{
    public class ExcelCreator : IDocumentCreator
    {
        public IFileRepository FileRepo { get; set; }
        private async Task<CommitEntity> GetCommit(string id)
        {
            var repo = new CommitRepository();

            var commit = await repo.GetCommitById(id);

            return commit;
        }
        private async Task<EmployeeEntity> GetEmployee(string id)
        {
            var repo = new EmployeesRepository();

            var emp = await repo.GetEmployeeById(id);

            return emp;
        }
        private double CalculateEmployeeTime(string employeeId, List<string> commits, DateTime startDate, DateTime endDate)
        {
            double time = 0;
            foreach (var commitId in commits)
            {
                var commit = GetCommit(commitId).Result;
                if (commit.Employee == employeeId
                    && commit.CommitDate >= startDate
                    && commit.CommitDate <= endDate)
                {
                    time += commit.TimeWorked;
                }
            }
            return time;
        }

        public void GenerateEmployeeReportExcel(EmployeeEntity employee, DateTime dateFrom, DateTime dateTo)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = new ExcelFile();

            int row = 4, col = 1;

            var repo = new ProjectRepository();
            var projects = repo.GetAllProjects().Result;

            var worksheet = workbook.Worksheets.Add(employee.FirstName + "_" + employee.LastName + " work_report");
            var c = new CellStyle();
            c.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Cells.Style = c;
            worksheet.Columns[1].Width = 35 * 256;
            //emp row
            worksheet.Cells[1, 0].Value = "Employee";
            worksheet.Cells[1, 0].Style.Font.Weight = ExcelFont.MaxWeight;
            worksheet.Cells[1, col].Value = employee.FirstName + " " + employee.LastName;
            row++;

            //date row
            worksheet.Cells[2, 0].Value = "Date";
            worksheet.Cells[2, 0].Style.Font.Weight = ExcelFont.MaxWeight;
            worksheet.Cells[2, col].Value = String.Format("{0:yyy.MM.dd} - {0:yyy.MM.dd}", dateFrom, dateTo);
            row++;
            double totalTime = 0;

            foreach (var project in projects)
            {
                if (project.Employees.Contains(employee.Id))
                {
                    //name row  
                    worksheet.Cells[row, col].Value = "Project";
                    worksheet.Cells[row, col].Style.Font.Weight = ExcelFont.MaxWeight;

                    worksheet.Cells[row, col + 1].Value = project.Name;
                    worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 3).Merged = true;
                    row++;
                    //title row
                    worksheet.Cells[row, col].Value = "Commits";
                    worksheet.Cells.GetSubrangeAbsolute(row, col, row, col + 3).Merged = true;
                    worksheet.Cells[row, col].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                    row++;
                    //info row
                    worksheet.Cells[row, col].Value = "Description";
                    worksheet.Cells[row, col].Style.Font.Weight = ExcelFont.MaxWeight;
                    worksheet.Cells[row, col].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

                    worksheet.Cells[row, col + 1].Value = "Time worked (h)";
                    worksheet.Cells[row, col + 1].Style.Font.Weight = ExcelFont.MaxWeight;
                    worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 3).Merged = true;
                    worksheet.Cells[row, col + 1].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                    row++;

                    foreach (var commId in project.Commits)
                    {
                        var commit = GetCommit(commId).Result;
                        if (commit.Employee == employee.Id)
                        {
                            if (commit.CommitDate >= dateFrom && commit.CommitDate <= dateTo)
                            {
                                worksheet.Cells[row, col].Value = commit.Description;
                                worksheet.Cells[row, col].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

                                worksheet.Cells[row, col + 1].Value = commit.TimeWorked;
                                worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 3).Merged = true;
                                totalTime += commit.TimeWorked;
                                worksheet.Cells[row, col + 1].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                                row++;
                            }
                        }
                    }
                    row++;
                    worksheet.Cells[row, col].Value = "Total employee work time (h)";
                    worksheet.Cells[row, col].Style.Font.Weight = ExcelFont.MaxWeight;

                    worksheet.Cells[row, col + 1].Value = totalTime;
                    worksheet.Cells[row, col + 1].Style.Font.Weight = ExcelFont.MaxWeight;
                    worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 3).Merged = true;
                }
            }
            string fileName = @"" + employee.FirstName + "_" + employee.LastName + "_"
            + String.Format("{0:d}", dateFrom) + "_"
            + String.Format("{0:d}", dateTo) + "_report.xlsx";


            workbook.Save(fileName);
        }

        public void GenerateProjectReportExcel(ProjectEntity project, DateTime dateFrom, DateTime dateTo)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = new ExcelFile();
            int row = 4, col = 1;
            double totalProjectTime = 0;

            var worksheet = workbook.Worksheets.Add(project.Name + " work_report");
            var c = new CellStyle();
            c.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Cells.Style = c;
            worksheet.Columns[1].Width = 35 * 256;
            //date row
            worksheet.Cells[1, 0].Value = "Date";
            worksheet.Cells[1, 0].Style.Font.Weight = ExcelFont.MaxWeight;
            worksheet.Cells[1, col].Value = String.Format("{0:yyy.MM.dd} - {0:yyy.MM.dd}", dateFrom, dateTo);
            row++;
            //project row
            worksheet.Cells[row, col].Value = "Project";
            worksheet.Cells[row, col].Style.Font.Weight = ExcelFont.MaxWeight;
            worksheet.Cells[row, col + 1].Value = project.Name;
            worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 3).Merged = true;
            row++;
            //title row  
            worksheet.Cells[row, col].Value = "Employee name";
            worksheet.Cells[row, col].Style.Font.Weight = ExcelFont.MaxWeight;
            worksheet.Cells[row, col].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

            worksheet.Cells[row, col + 1].Value = "Time worked (h)";
            worksheet.Cells[row, col + 1].Style.Font.Weight = ExcelFont.MaxWeight;
            worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 3).Merged = true;
            worksheet.Cells[row, col + 1].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
            row++;

            foreach (var empId in project.Employees)
            {
                double empWorkTime = 0;
                empWorkTime = CalculateEmployeeTime(empId, project.Commits, dateFrom, dateTo);
                var employee = GetEmployee(empId).Result;

                worksheet.Cells[row, col].Value = employee.FirstName + " " + employee.LastName;
                worksheet.Cells[row, col].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

                worksheet.Cells[row, col + 1].Value = empWorkTime;
                worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 3).Merged = true;
                worksheet.Cells[row, col + 1].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                row++;
                totalProjectTime += empWorkTime;
            }
            row++;
            worksheet.Cells[row, col].Value = "Total work time (h)";
            worksheet.Cells[row, col].Style.Font.Weight = ExcelFont.MaxWeight;

            worksheet.Cells[row, col + 1].Value = totalProjectTime;
            worksheet.Cells[row, col + 1].Style.Font.Weight = ExcelFont.MaxWeight;
            worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 3).Merged = true;

            string fileName = @"" + project.Name + "_"
            + String.Format("{0:d}", dateFrom) + "_"
            + String.Format("{0:d}", dateTo) + "_report.xls";

            workbook.Save(fileName);
        }

        public void GenerateMultipleProjectsReportExcel(List<ProjectEntity> projects, DateTime dateFrom, DateTime dateTo)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = new ExcelFile();
            int row = 4, col = 1;
            double totalProjectTime = 0;

            var worksheet = workbook.Worksheets.Add("projects work_report");
            var c = new CellStyle();
            c.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Cells.Style = c;

            //date row
            worksheet.Cells[1, 0].Value = "Date";
            worksheet.Cells[1, 0].Style.Font.Weight = ExcelFont.MaxWeight;
            worksheet.Cells[1, col].Value = String.Format("{0:yyy.MM.dd} - {0:yyy.MM.dd}", dateFrom, dateTo);

            foreach (var project in projects)
            {
                worksheet.Columns[col].Width = 35 * 256;
                //project row
                worksheet.Cells[row, col].Value = "Project name";
                worksheet.Cells[row, col].Style.Font.Weight = ExcelFont.MaxWeight;

                worksheet.Cells[row, col + 1].Value = project.Name;
                worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 2).Merged = true;
                row++;
                //title row  
                worksheet.Cells[row, col].Value = "Employee name";
                worksheet.Cells[row, col].Style.Font.Weight = ExcelFont.MaxWeight;
                worksheet.Cells[row, col].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

                worksheet.Cells[row, col + 1].Value = "Time worked (h)";
                worksheet.Cells[row, col + 1].Style.Font.Weight = ExcelFont.MaxWeight;
                worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 2).Merged = true;
                worksheet.Cells[row, col + 1].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                row++;

                foreach (var empId in project.Employees)
                {
                    double empWorkTime = 0;
                    empWorkTime = CalculateEmployeeTime(empId, project.Commits, dateFrom, dateTo);
                    var employee = GetEmployee(empId).Result;

                    worksheet.Cells[row, col].Value = employee.FirstName + " " + employee.LastName;
                    worksheet.Cells[row, col].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

                    worksheet.Cells[row, col + 1].Value = empWorkTime;
                    worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 2).Merged = true;
                    worksheet.Cells[row, col + 1].Style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
                    row++;
                    totalProjectTime += empWorkTime;
                }
                row++;
                worksheet.Cells[row, col].Value = "Total project work time (h)";
                worksheet.Cells[row, col].Style.Font.Weight = ExcelFont.MaxWeight;

                worksheet.Cells[row, col + 1].Value = totalProjectTime;
                worksheet.Cells[row, col + 1].Style.Font.Weight = ExcelFont.MaxWeight;
                worksheet.Cells.GetSubrangeAbsolute(row, col + 1, row, col + 2).Merged = true;
                col = col + 5;
                row = 4;
                totalProjectTime = 0;
            }
            string fileName = @""
            + String.Format("{0:d}", dateFrom) + "_"
            + String.Format("{0:d}", dateTo) + "_projects_report.xlsx";

            workbook.Save(fileName);
        }

        public Task GenerateAbsenceWordAsync(EmployeeEntity employee, string reason, AbsenceRequestEntity absenceRequest, string end, string absenceType, string days)
        {
            throw new NotImplementedException();
        }

        public void GenerateEmploeeReportWord(EmployeeEntity employee, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void GenerateProjectsReportWord(List<ProjectEntity> projects, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void GenerateSelectedProjectReportWord(ProjectEntity project, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
