using HrApp.Domain;
using HrApp.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp.Contracts
{
    public interface ICommitRepository
    { 
        Task<string> InsertCommit(Commit commit);
        Task<List<CommitEntity>> GetCommits(Employee employee);

    }
}
