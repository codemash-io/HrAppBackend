using CodeMash.Client;
using CodeMash.Project.Services;
using CodeMash.Repository;
using Isidos.CodeMash.ServiceContracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class ImportFileRepository:IImportFileRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<ImportFileEintity> GetImportFileById(string id)
        {
            var repo = new CodeMashRepository<ImportFileEintity>(Client);
            var file = await repo.FindOneByIdAsync(id, new DatabaseFindOneOptions() { });
            return file;

        }

        public async Task UpdateErrorStatus(string recordId, bool status)
        {
            var repo = new CodeMashRepository<ImportFileEintity>(Client);

            await repo.UpdateOneAsync(
                x => x.Id == recordId,
                Builders<ImportFileEintity>.Update.Set(x => x.Successful, status),
                new DatabaseUpdateOneOptions()
            );
        }

        public async Task InsertErrorFile(string recordId, byte[] stream)
        {
            var filesService = new CodeMashFilesService(Client);

            var response = await filesService.UploadRecordFileAsync(stream, "ErrorLog.txt",
            new UploadRecordFileRequest
            {
                RecordId = recordId,
                CollectionName = "rivile-imports",
                UniqueFieldName = "error_file"
            });


        }

    }
}
