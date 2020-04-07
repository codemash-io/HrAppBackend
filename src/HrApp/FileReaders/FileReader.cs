using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HrApp
{
    public class FileReader:IFileReader
    {
        public async Task ProcessFile(Stream fileStream,string type) {
            VacationBalance vacationBalance = new VacationBalance();
            IWorkbook book=null;
            ICell cellName = null;
            ICell cellTotal = null;
            ICell cellUsed = null;
            ICell cellLeft = null;
            if (type==".xsl")
            {
                book = new HSSFWorkbook(fileStream);
            }
            if (type == ".xslx")
            {
                book = new HSSFWorkbook(fileStream);
            }
            
            var sheet = (XSSFSheet)book.GetSheetAt(0);
            int rowIndex = 6;  //--- SKIP FIRST ROW (index == 0) AS IT CONTAINS TEXT HEADERS
            int rowWithData = 0;
            int columns = 0;
            while (sheet.GetRow(rowIndex) != null)
            {
                columns = sheet.GetColumnWidth(rowIndex);

                cellName = sheet.GetRow(rowIndex).GetCell(1);
                cellName.SetCellType(CellType.String);
                cellTotal = sheet.GetRow(rowIndex).GetCell(7);
                cellTotal.SetCellType(CellType.String);
                cellUsed = sheet.GetRow(rowIndex).GetCell(8);
                cellUsed.SetCellType(CellType.String);
                cellLeft = sheet.GetRow(rowIndex).GetCell(9);
                cellLeft.SetCellType(CellType.String);
                vacationBalance.Employees.Add(new Personal() { 
                Employee = cellName.StringCellValue,
                Total = double.Parse(cellTotal.StringCellValue),
                Used = double.Parse(cellUsed.StringCellValue),
                Left = double.Parse(cellLeft.StringCellValue)
                });


                if (rowWithData==42)
                {
                    rowWithData = 0;
                    rowIndex += 6;
                }
                else
                {
                    rowWithData++;
                    rowIndex++;
                }                
            }

        }

    }
}
