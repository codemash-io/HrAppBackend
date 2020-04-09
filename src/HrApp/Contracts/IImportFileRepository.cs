using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IImportFileRepository
    {
        Task<ImportFileEintity> GetImportFileById(string id);
        Task UpdateErrorStatus(string recordId, bool status);
        Task InsertErrorFile(string recordId, byte[] stream);
    }
}
