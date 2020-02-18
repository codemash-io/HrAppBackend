using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TimeTrackerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Set the API Endpoint to Graph 'me' endpoint
        string graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

        //Set the scope for API call to user.read
        string[] scopes = new string[] { "user.read" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Call AcquireToken - to acquire a token requiring user to sign-in
        /// </summary>
        private async void CallGraphButton_Click(object sender, RoutedEventArgs e)
        {
             AuthenticationResult authResult = null;
             var app = App.PublicClientApp;

             ResultText.Text = string.Empty;
             TokenInfoText.Text = string.Empty;

             var accounts = await app.GetAccountsAsync();
             var firstAccount = accounts.FirstOrDefault();

             try
             {
                 authResult = await app.AcquireTokenSilent(scopes, firstAccount)
                     .ExecuteAsync();
             }
             catch (MsalUiRequiredException ex)
             {
                 // A MsalUiRequiredException happened on AcquireTokenSilent. 
                 // This indicates you need to call AcquireTokenInteractive to acquire a token
                 System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                 try
                 {
                     authResult = await app.AcquireTokenInteractive(scopes)
                         .WithAccount(accounts.FirstOrDefault())                     
                         .WithPrompt(Prompt.SelectAccount)
                         .ExecuteAsync();
                 }
                 catch (MsalException msalex)
                 {
                     ResultText.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                 }
             }
             catch (Exception ex)
             {
                 ResultText.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                 return;
             }

             if (authResult != null)
             {
                 UpdateResource("username", authResult.Account.Username);
                 LoggerWindow loggerWindow = new LoggerWindow();
                 loggerWindow.Owner = this;
                 this.Hide();
                 loggerWindow.ShowDialog();


                 ResultText.Text = await GetHttpContentWithToken(graphAPIEndpoint, authResult.AccessToken);
                 DisplayBasicTokenInfo(authResult);
             }
             
        }
        private void UpdateResource(string name, string resource)
        {
            Application.Current.Resources.Remove(name);
            Application.Current.Resources.Add(name, resource);
        }

        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        public async Task<string> GetHttpContentWithToken(string url, string token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response;
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Display basic information contained in the token
        /// </summary>
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            TokenInfoText.Text = "";
            if (authResult != null)
            {
                TokenInfoText.Text += $"Username: {authResult.Account.Username}" + Environment.NewLine;
                TokenInfoText.Text += $"Token Expires: {authResult.ExpiresOn.ToLocalTime()}" + Environment.NewLine;
            }
        }
    }
}
