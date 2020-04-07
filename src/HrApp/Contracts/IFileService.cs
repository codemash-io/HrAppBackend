using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IFileService
    {
        Task ReadExcel(ImportFileEintity importFile);

    }
}
