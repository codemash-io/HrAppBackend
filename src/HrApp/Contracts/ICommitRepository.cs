using HrApp.Domain;
using HrApp.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp.Contracts
{
    public interface ICommitRepository
    { 
        Task<CommitEntity> InsertCommit(Commit commit);
        Task<List<CommitEntity>> GetCommitsByEmployee(EmployeeEntity employee);

    }
}
