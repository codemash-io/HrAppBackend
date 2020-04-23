using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HrApp
{
    public class ESingnatureService : IESignatureService
    {
        public IFileRepository FileRepo { get; set; }
        public IESignRepository SignitureRepo { get; set; }

        private const string accessToken = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiNjgxODVmZjEtNGU1MS00Y2U5LWFmMWMtNjg5ODEyMjAzMzE3In0.AQoAAAABAAUABwAA3YNM6-XXSAgAAB2nWi7m10gCABdRwK_cjJ9Hg_6dPbSj204VAAEAAAAYAAEAAAAFAAAADQAkAAAAM2UxMjZjY2UtMWEzNC00NGZlLWI4N2EtNTM0Y2Q5NGRiMjUwIgAkAAAAM2UxMjZjY2UtMWEzNC00NGZlLWI4N2EtNTM0Y2Q5NGRiMjUwMAAA3YNM6-XXSBIAAQAAAAsAAABpbnRlcmFjdGl2ZQ.gu4dmPAASv2lqx9NayWx9ItyjyPYC0rvKs1QH6tSpxphUUtvUZWbNEiqeNwbdJVahJnLgivqk2mMQcWkHXFmsnWkfIbzu6KSI5C-FvktOeQeUyjn_054IdB05ch4QGCcJ7gwIu_nmfNCnJvjXVofPLCoUpHqj9uJ4anR0oLoWDQNyQBgE5iEqNfYmtcEcZiEYIFLXEOma_i83ebi3uxI_dJeIZxZ8v7EV8t0jNPKyllD0u9KOxMwKRLuEpl89RglVSmcdjLuBvczJhtonxRcjQP_SyH_Jp7cnj4HqYJKjDrWtf7xfOPt440E07v_ehguS-DrzAikmF7X5TjixZ9FYw";
         private const string accountId = "3e126cce-1a34-44fe-b87a-534cd94db250";
       // private const string accountId = "a963c14f-68fd-49ae-8d90-205e4c887046";
        private const string signerName = "Aurimas Valauskas";
        private const string signerEmail = "valauskas.aurimas@gmail.com";
        private const string docName = "foodai.pdf";

        private const string signerClientId = "1000";
        private const string basePath = "https://demo.docusign.net";

        // Change the port number in the Properties / launchSettings.json file:
        //     "iisExpress": {
        //        "applicationUrl": "http://localhost:5050",
        private const string returnUrl = "http://localhost:44388";
        public async Task<ViewUrl> OnPostAsync()
        {
            var eSignSettings = new ESignSettings();
            eSignSettings.CheckToken();
            //await SignitureRepo.GetAccessToken();

            var fileBytes =  await FileRepo.GetFileBytes("ab260cff-7df8-4b56-ae90-b646e487bb8f");

            EnvelopeDefinition envelope = SignitureRepo.MakeEnvelope(signerEmail, signerName, signerClientId, fileBytes);

            // Step 2. Call DocuSign to create the envelope                   
            var config = new Configuration(new ApiClient(basePath));
            config.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(config);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, envelope);
            string envelopeId = results.EnvelopeId;

            RecipientViewRequest viewRequest = SignitureRepo.MakeRecipientViewRequest(signerEmail, signerName, signerClientId, returnUrl, returnUrl);
            // call the CreateRecipientView API
            ViewUrl results1 = envelopesApi.CreateRecipientView(accountId, envelopeId, viewRequest);

            // Step 4. Redirect the user to the Signing Ceremony
            // Don't use an iFrame!
            // State can be stored/recovered using the framework's session or a
            // query parameter on the returnUrl (see the makeRecipientViewRequest method)
            var redirectUrl = results1.Url;
            return null;
        }


    }
}
