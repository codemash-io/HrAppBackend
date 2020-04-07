using CodeMash.Client;
using CodeMash.Repository;
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

    }
}
