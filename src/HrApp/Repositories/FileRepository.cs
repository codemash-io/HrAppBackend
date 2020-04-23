using CodeMash.Client;
using CodeMash.Code.Services;
using CodeMash.Project.Services;
using Isidos.CodeMash.ServiceContracts;
using Newtonsoft.Json.Linq;
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

        public async Task<string> UploadFile(string fileName, DocumentCore doc,string abscenceId)
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

        public string GetFileId(ImportFileEintity file)
        {
             //all file names list
             var names = file.VacationBalanceFile;
             // converting one of the file to string
             var source = names[0].ToString();
             //parsing formated string json
             dynamic data = JObject.Parse(source);
             //accessing json fields
             string fileId = data.id;
             string fileType = data.originalFileName;
             //var logger = Logger.GetLogger();
             //if (!fileType.EndsWith(format))
             //{

             //    logger.Log("Bad file format! File needs to be " + format);
             //    return "";
             //}

             return fileId;
           
        }
        public string GetFileId(object file)
        {
            var source = file.ToString();
            //parsing formated string json
            dynamic data = JObject.Parse(source);
            //accessing json fields
            string fileId = data.fileId;
            string fileType = data.originalFileName;
            return fileId;
        }
        public string GetPhotoId(object photo)
        {
            var source = photo.ToString();
            //parsing formated string json
            dynamic data = JObject.Parse(source);
            //accessing json fields
            string fileId = data.id;
            string fileType = data.originalFileName;
            return fileId;
        }


        public async Task<Stream> GetFile(string fileId)
        {
            var filesRepo = new CodeMashFilesService(Client);

            var response = await filesRepo.GetFileStreamAsync(new GetFileRequest()
            {
                FileId = fileId,
                ProjectId = Settings.ProjectId
            });
            return response;
        }

        public async Task<byte[]> GetFileBytes(string fileId)
        {
            var filesRepo = new CodeMashFilesService(Client);           
            var response = await filesRepo.GetFileBytesAsync(new GetFileRequest()
            {
                FileId = fileId,
                ProjectId = Settings.ProjectId
            });
            return response;
        }

        public async Task<string> GenerateLunchOrderReport(string data, string template)
        {
            var codeService = new CodeMashCodeService(Client);

            var response = await codeService.ExecuteFunctionAsync(new ExecuteFunctionRequest
            {
                Id = Guid.Parse(template),
                Tokens = new Dictionary<string, string>()
               {
                   { "order", data}
               }
            });
            var fileId = GetFileId(response.Result);

            return fileId;
        }

        public async Task<string> GenerateAbscenseFileWithSignature(AbsenceRequestEntity absence, string employee, string signature)
        {
            var codeService = new CodeMashCodeService(Client);

            var response = await codeService.ExecuteFunctionAsync(new ExecuteFunctionRequest
            {
                Id = Guid.Parse("e970faaa-4e60-42b8-a4d5-a9d37dbf6320"),
                Tokens = new Dictionary<string, string>()
               {
                   { "Signature", signature},
                   { "Employee", employee},
                   { "Type", absence.Type},
                   { "From", absence.AbsenceStart.ToString()},
                   { "To", absence.AbsenceEnd.ToString()},
               }
            });;
            var fileId = GetFileId(response.Result);

            return fileId;
        }


    }
}

