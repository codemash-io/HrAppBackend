using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HrApp
{
    public class ESignRepository: IESignRepository
    {
        private const string basePath = "https://account.docusign.com/oauth/auth";
    //    private const string basePath = "https://demo.docusign.net/restapi";
        private const string returnUrl = "https://api.codemash.io/v1/serverless/functions/777db916-a4e6-4c5e-9957-f77f0efdc6b5/execute";

         private string accessToken = Settings.accessTokenESign;
         string accountId = Settings.accountIdESign;


        public async Task GetAccessToken()
        {
            var tokenEndPoint = "https://account-d.docusign.com/";

            var parameters = new Dictionary<string, string> { 
                { "response_type", "code" },
                { "scope", "signature" }, 
                { "client_id", "3e126cce-1a34-44fe-b87a-534cd94db250" }, 
                { "state", "random" }, 
                { "redirect_uri", "https://localhost:44388/" } };


            using (var client = new HttpClient())
            {
                using (var encodedContent = new FormUrlEncodedContent(parameters))
                {
                 
                    var response = client.PostAsync(tokenEndPoint, encodedContent).Result;
                    response.EnsureSuccessStatusCode();
                     var resultString = response.Headers.ToString();
                    //  response.EnsureSuccessStatusCode();
                    //string responseUri = response.RequestMessage..ToString();
                    // Console.Out.WriteLine(responseUri);

                    //  var resultJson = JObject.Parse(resultString);
                }

            }             
         
        }

       














        public async Task<ViewUrl> OnPostAsync(string fileName,string fileId, string signerEmail, string signerName, string signerClientId, byte[] fileBytes)
        {
            string file="";
            // Embedded Signing Ceremony
            // 1. Create envelope request obj
            // 2. Use the SDK to create and send the envelope
            // 3. Create Envelope Recipient View request obj
            // 4. Use the SDK to obtain a Recipient View URL
            // 5. Redirect the user's browser to the URL
            int charLocation = fileName.IndexOf(".", StringComparison.Ordinal);

            if (charLocation > 0)
            {
                 file = fileName.Substring(0, charLocation);
            }
            // 1. Create envelope request object
            //    Start with the different components of the request
            Document document = new Document
            {
                DocumentBase64 = Convert.ToBase64String(fileBytes),
                Name = file,
                FileExtension = fileName,
                DocumentId = fileId
            };
            Document[] documents = new Document[] { document };

            // Create the signer recipient object 
            Signer signer = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                ClientUserId = signerClientId,
                RecipientId = signerClientId,
                RoutingOrder = "1"
            };

            // Create the sign here tab (signing field on the document)
            SignHere signHereTab = new SignHere
            {
                DocumentId = fileId,
                PageNumber = "1",
                RecipientId = "1",
                TabLabel = "Sign Here Tab",
                XPosition = "195",
                YPosition = "147"
            };
            SignHere[] signHereTabs = new SignHere[] { signHereTab };

            // Add the sign here tab array to the signer object.
            signer.Tabs = new Tabs { SignHereTabs = new List<SignHere>(signHereTabs) };
            // Create array of signer objects
            Signer[] signers = new Signer[] { signer };
            // Create recipients object
            Recipients recipients = new Recipients { Signers = new List<Signer>(signers) };
            // Bring the objects together in the EnvelopeDefinition
            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition
            {
                EmailSubject = "Please sign the document",
                Documents = new List<Document>(documents),
                Recipients = recipients,
                Status = "sent"
            };

            ApiClient apiClient = new ApiClient(basePath);
            apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, envelopeDefinition);

            string envelopeId = results.EnvelopeId;
            RecipientViewRequest viewOptions = new RecipientViewRequest
            {
                ReturnUrl = returnUrl,
                ClientUserId = signerClientId,
                AuthenticationMethod = "none",
                UserName = signerName,
                Email = signerEmail
            };
            ViewUrl viewUrl = envelopesApi.CreateRecipientView(accountId, envelopeId, viewOptions);
            return viewUrl;

        }




        public EnvelopeDefinition MakeEnvelope(string signerEmail, string signerName,string signerId, byte[] file)
        {
            // Data for this method
            // signerEmail 
            // signerName
            // signerClientId -- class global
            // Config.docPdf


            

            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition();
            envelopeDefinition.EmailSubject = "Please sign this document";
            Document doc1 = new Document();

            String doc1b64 = Convert.ToBase64String(file);

            doc1.DocumentBase64 = doc1b64;
            doc1.Name = "fooodu vardas"; // can be different from actual file name
            doc1.FileExtension = "pdf";
            doc1.DocumentId = "3";

            // The order in the docs array determines the order in the envelope
            envelopeDefinition.Documents = new List<Document> { doc1 };

            // Create a signer recipient to sign the document, identified by name and email
            // We set the clientUserId to enable embedded signing for the recipient
            // We're setting the parameters via the object creation
            Signer signer1 = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                ClientUserId = signerId,
                RecipientId = "1"
            };

            // Create signHere fields (also known as tabs) on the documents,
            // We're using anchor (autoPlace) positioning
            //
            // The DocuSign platform seaches throughout your envelope's
            // documents for matching anchor strings.
            SignHere signHere1 = new SignHere
            {
                AnchorString = "/sn1/",
                AnchorUnits = "pixels",
                AnchorXOffset = "10",
                AnchorYOffset = "20"
            };
            // Tabs are set per recipient / signer
            Tabs signer1Tabs = new Tabs
            {
                SignHereTabs = new List<SignHere> { signHere1 }
            };
            signer1.Tabs = signer1Tabs;

            // Add the recipient to the envelope object
            Recipients recipients = new Recipients
            {
                Signers = new List<Signer> { signer1 }
            };
            envelopeDefinition.Recipients = recipients;

            // Request that the envelope be sent by setting |status| to "sent".
            // To request that the envelope be created as a draft, set to "created"
            envelopeDefinition.Status = "sent";

            return envelopeDefinition;
        }

        public RecipientViewRequest MakeRecipientViewRequest(string signerEmail, string signerName,string signerId, string returnUrl, string baseUrl)
        {
            // Data for this method
            // signerEmail 
            // signerName
            // dsPingUrl -- class global
            // signerClientId -- class global
            // dsReturnUrl -- class global


            RecipientViewRequest viewRequest = new RecipientViewRequest();
            // Set the url where you want the recipient to go once they are done signing
            // should typically be a callback route somewhere in your app.
            // The query parameter is included as an example of how
            // to save/recover state information during the redirect to
            // the DocuSign signing ceremony. It's usually better to use
            // the session mechanism of your web framework. Query parameters
            // can be changed/spoofed very easily.
            viewRequest.ReturnUrl = returnUrl;

            // How has your app authenticated the user? In addition to your app's
            // authentication, you can include authenticate steps from DocuSign.
            // Eg, SMS authentication
            viewRequest.AuthenticationMethod = "none";

            // Recipient information must match embedded recipient info
            // we used to create the envelope.
            viewRequest.Email = signerEmail;
            viewRequest.UserName = signerName;
            viewRequest.ClientUserId = signerId;

            // DocuSign recommends that you redirect to DocuSign for the
            // Signing Ceremony. There are multiple ways to save state.
            // To maintain your application's session, use the pingUrl
            // parameter. It causes the DocuSign Signing Ceremony web page
            // (not the DocuSign server) to send pings via AJAX to your
            // app,
            viewRequest.PingFrequency = "600"; // seconds
                                               // NOTE: The pings will only be sent if the pingUrl is an https address
            viewRequest.PingUrl = baseUrl; // optional setting

            return viewRequest;
        }
    }
}
