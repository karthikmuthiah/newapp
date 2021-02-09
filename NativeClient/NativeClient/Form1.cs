using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Globalization;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NativeClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

        AuthenticationContext authContext;
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        Uri redirectUri = new Uri(ConfigurationManager.AppSettings["ida:RedirectUri"]);

        private static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        //
        // To authenticate to the To Do list service, the client needs to know the service's App ID URI.
        // To contact the To Do list service we need it's URL as well.
        //
        private static string todoListResourceId = ConfigurationManager.AppSettings["todo:TodoListResourceId"];
        private static string todoListBaseAddress = ConfigurationManager.AppSettings["todo:TodoListBaseAddress"];


        private void button1_Click(object sender, EventArgs e)
        {
            GetTodoList();
        }

        private async void GetTodoList()
        {
            AuthenticationResult result = null;
            HttpClient httpClient = new HttpClient();
            authContext = new AuthenticationContext(authority);
            result = await authContext.AcquireTokenAsync(todoListResourceId, clientId, redirectUri, new PlatformParameters(PromptBehavior.Auto));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            
            // Call the To Do list service.
            HttpResponseMessage response = await httpClient.GetAsync(todoListBaseAddress + "/api/values/4");
         
            //MessageBox.Show(response.RequestMessage.ToString());
            string s = await response.Content.ReadAsStringAsync();
            MessageBox.Show(s);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
