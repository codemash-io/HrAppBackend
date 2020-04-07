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


        public async Task ReadExcel(ImportFileEintity importFile)
        {
            var fileId = FileRepo.GetFileId(importFile);
            //jei blogai


            //gerai:

            var vacationStream = await FileRepo.GetFile(fileId);
            using (var fileStream = new MemoryStream())
            {
                vacationStream.CopyTo(fileStream);
                await FileReader.ProcessFile(fileStream, ".xls");

            }
        }
    }
}
