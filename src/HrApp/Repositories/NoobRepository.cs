using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class NoobRepository:INoobRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task<NoobFormEntity> GetNoobFormById(string id)
        {
            var repo = new CodeMashRepository<NoobFormEntity>(Client);
            NoobFormEntity form;
            try
            {
                form = await repo.FindOneByIdAsync(id, new DatabaseFindOneOptions() { });
            }
            catch
            {
                throw new BusinessException("Photo is missing");
            }


            return form;

        }

    }
}
