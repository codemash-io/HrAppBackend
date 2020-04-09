using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IFileReader
    {
        VacationBalance ProcessFile(Stream fileStream, ref bool hasError);

    }
}
