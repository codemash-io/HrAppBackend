using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface INoobRepository
    {
        Task<NoobFormEntity> GetNoobFormById(string id);
    }
}
