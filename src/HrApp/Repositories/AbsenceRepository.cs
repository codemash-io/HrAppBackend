using CodeMash.Client;
using CodeMash.Repository;
using Isidos.CodeMash.ServiceContracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HrApp
{
    public class AbsenceRepository : IAbsenceRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task<AbsenceRequestEntity> GetAbsenceById(string id)
        {
            var repo = new CodeMashRepository<AbsenceRequestEntity>(Client);
            var absence = await repo.FindOneByIdAsync(id, new DatabaseFindOneOptions() {  });
            return absence;
                       
        }
        public async Task<AbsenceRequestEntity> GetAbsenceByIdWithTypes(string id)
        {
            var repo = new CodeMashRepository<AbsenceRequestEntity>(Client);
            var absence = await repo.FindOneByIdAsync(id, new DatabaseFindOneOptions() { IncludeTermNames = true });
            return absence;

        }
        public async Task<string> GetAbsenceByIdWithNames(string id)
        {
            var termEntitis = new TermEntitis();


            var repo = new CodeMashTermsService(Client);

            //FilterDefinition<TermEntitis> filter = "{ id: "+id+" }";

            var terms =await repo.FindAsync("absence-type", x=>true, new TermsFindOptions(){});

            var term = terms.Items.Where(x => x.Id == id).Select(x => x.Name).ToList();

             //var d = new FindOneRequest() { IncludeTermNames = true };

            // return absence;
             
            return term[0];
        }

        public async Task InsertAbsenceWithSignature(string fileId, string entityId)
        {
            var repo = new CodeMashRepository<AbsenceRequestEntity>(Client);
            var update = Builders<AbsenceRequestEntity>.Update
                .Set("file_with_signature", new List<string> { fileId })
                .Set("signatureimage", new List<string>());
            var response = await repo.UpdateOneAsync(
                 entityId,
                 update,
                 new DatabaseUpdateOneOptions() { WaitForFileUpload = true }
            );


        }


    }
}
