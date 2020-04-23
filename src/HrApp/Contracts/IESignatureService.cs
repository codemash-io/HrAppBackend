using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IESignatureService
    {
        // Task<ViewUrl> CreateESignRequest(string fileId, string employeeGuid);
        Task<ViewUrl> OnPostAsync();
    }
}
