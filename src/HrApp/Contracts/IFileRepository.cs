using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IFileRepository
    {
        Task<string> UploadFile(string fileName, DocumentCore doc, string abscenceId);
       // Task GetFileId();
    }
}
