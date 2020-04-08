using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class FileService:IFileService
    {
        public IImportFileRepository ImportFileRepo { get; set; }
        public IFileRepository FileRepo { get; set; }
        public IFileReader FileReader { get; set; }
        public IEmployeesRepository EmployeeRepo { get; set; }


        public async Task ReadExcel(ImportFileEintity importFile)
        {
            var hasError = false;
            var fileId = FileRepo.GetFileId(importFile);

            if (fileId==null)
            {
                throw new BusinessException("File isn't uploaded");
            }
            var vacationStream = await FileRepo.GetFile(fileId);

            using (var fileStream = new MemoryStream())
            {
                vacationStream.CopyTo(fileStream);
                fileStream.Seek(0, SeekOrigin.Begin);
                var vacations = await FileReader.ProcessFile(fileStream);
                if (!hasError)
                {
                    foreach (var employee in vacations.Employees)
                    {
                        if (!hasError)
                        {
                            hasError = await EmployeeRepo.UpdateVacationBalance(employee);
                        }
                        else
                        {
                            await EmployeeRepo.UpdateVacationBalance(employee);
                        }
                    }
                    if (hasError)
                    {
                        await UploadErrorFile(importFile.Id);
                        await ImportFileRepo.UpdateErrorStatus(importFile.Id, false);
                    }
                    else
                    {
                        await ImportFileRepo.UpdateErrorStatus(importFile.Id, true);
                    }
                }
                else {
                    await UploadErrorFile(importFile.Id);
                    await ImportFileRepo.UpdateErrorStatus(importFile.Id, false);
                }




            }             
        }
        private async Task UploadErrorFile(string recordId)
        {
            MemoryStream memoryStream = Logger.GetStream();
            if (memoryStream != null)
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                int count = 0;
                byte[] byteArray = new byte[memoryStream.Length];
                while (count < memoryStream.Length)
                {
                    byteArray[count++] = Convert.ToByte(memoryStream.ReadByte());
                }
                await ImportFileRepo.InsertErrorFile(recordId, byteArray);
            }
        }
    }
}
