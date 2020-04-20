using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IGraphFileRepository
    {
        Task<Drive> GetUserSelectedOrDefaultDrive(string userId, string driveId = null);

    }
}
