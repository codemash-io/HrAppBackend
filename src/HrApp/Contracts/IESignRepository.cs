using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public interface IESignRepository
    {
        Task<ViewUrl> OnPostAsync(string fileName, string fileId, string signerEmail, string signerName, string signerClientId, byte[] fileBytes);
        EnvelopeDefinition MakeEnvelope(string signerEmail, string signerName, string signerId, byte[] file);
        RecipientViewRequest MakeRecipientViewRequest(string signerEmail, string signerName, string signerId, string returnUrl, string baseUrl);
        Task GetAccessToken();

    }
}
