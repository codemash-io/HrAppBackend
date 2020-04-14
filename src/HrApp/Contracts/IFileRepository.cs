using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IFileRepository
    {
        Task<string> UploadFile(string fileName, DocumentCore doc, string abscenceId);
        string GetFileId(ImportFileEintity file);
        Task<Stream> GetFile(string fileId);
        string GetFileId(object photo);
        Task<byte[]> GetFileBytes(string fileId);
    }
}
