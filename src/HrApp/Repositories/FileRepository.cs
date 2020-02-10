using CodeMash.Client;
using CodeMash.Project.Services;
using Isidos.CodeMash.ServiceContracts;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class FileRepository : IFileRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<string> UploadFile(string fileName, DocumentCore doc, string abscenceId)
        {
            var filesService = new CodeMashFilesService(Client);
            string key;

            using (MemoryStream ms = new MemoryStream())
            {
                doc.Save(ms, new DocxSaveOptions());

                ms.Seek(0, SeekOrigin.Begin);

                byte[] byteArray = new byte[ms.Length];
                int count = ms.Read(byteArray, 0, 20);
                while (count < ms.Length)
                {
                    byteArray[count++] =
                        Convert.ToByte(ms.ReadByte());
                }
                var response = await filesService.UploadRecordFileAsync(byteArray, fileName,
                    new UploadRecordFileRequest
                    {
                        UniqueFieldName = "absence_description",
                        CollectionName = "absence-requests",
                        RecordId = abscenceId

                    }
                );
                key = response.Key;

            }
            return key;
        }
    }
}
