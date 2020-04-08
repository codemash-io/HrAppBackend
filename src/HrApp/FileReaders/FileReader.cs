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
        public async Task<VacationBalance> ProcessFile(Stream fileStream) {
            VacationBalance vacationBalance = new VacationBalance();
            IWorkbook book=null;
            ICell cellName = null;
            ICell cellTotal = null;
            ICell cellUsed = null;
            ICell cellLeft = null;

            book = new HSSFWorkbook(fileStream);
            var sheet = book.GetSheetAt(0);
            int rowIndex = 0;  
            double totalBalance;
            while (sheet.GetRow(rowIndex) != null)
            {
                cellTotal = sheet.GetRow(rowIndex).GetCell(6);
                cellTotal.SetCellType(CellType.String);
                
                cellName = sheet.GetRow(rowIndex).GetCell(1);
                cellName.SetCellType(CellType.String);
              //  double.TryParse(cellTotal.StringCellValue, out totalBalance)
                if (cellTotal.StringCellValue != "" && cellName.StringCellValue !="")
                {
                    if (cellTotal.StringCellValue != "Priklauso")
                    {
                        if (double.TryParse(cellTotal.StringCellValue, out totalBalance))
                        {
                            cellUsed = sheet.GetRow(rowIndex).GetCell(7);
                            cellUsed.SetCellType(CellType.String);
                            cellLeft = sheet.GetRow(rowIndex).GetCell(8);
                            cellLeft.SetCellType(CellType.String);
                            vacationBalance.Employees.Add(new Personal()
                            {
                                Employee = cellName.StringCellValue,
                                Total = totalBalance,
                                Used = double.Parse(cellUsed.StringCellValue),
                                Left = (int)double.Parse(cellLeft.StringCellValue),
                            });
                        }
                        else {
                            Logger logger = Logger.GetLogger();
                            var column = ((char)70).ToString();
                            var message = String.Format("Error in line: {0}, coll: {1} " + rowIndex, column);
                            logger.Log(message);                          
                        }

                        
                    }


                    rowIndex++;
                }
                else {
                    rowIndex += 5;
                }
                
            }
            return vacationBalance;
        }

    }
}
