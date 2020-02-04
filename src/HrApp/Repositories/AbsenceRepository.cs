﻿using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public class AbsenceRepository : IAbsenceRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        public async Task UpdateStatus()
        {
            var repo = new CodeMashRepository<AbsenceRequestEntity>(Client);
            //   var response = await repo.UpdateOneAsync("5e32c947f06da80001a12f4a", ,new DatabaseInsertOneOptions());
            AbsenceRequestStatus status = AbsenceRequestStatus.Approved;
            try
            {
                 repo.UpdateOne(x => x.Id == "5e32c947f06da80001a12f4a",
                  Builders<AbsenceRequestEntity>.Update.Set(x => x.Status, status.ToString()), null);
            }
            catch (Exception ex)
            { 
            
            }
           
            
        }
    }
}
