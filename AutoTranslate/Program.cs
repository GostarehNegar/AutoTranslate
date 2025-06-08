using Microsoft.Xrm.Sdk.Client;
using System;
using System.Linq;
using System.ServiceModel.Description;

namespace AutoTranslate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var service = GetOrganizationServiceProxy("http://gncrm2019/Test/", "gnco\\hafezi","123@qwe");
            var sourceContext = new OrganizationServiceContext(service);
            var query = sourceContext.CreateQuery("gn_translationentry");
            var items = query.Take(10).ToArray();
            foreach(var item in items)
            {
                Console.WriteLine(item.GetAttributeValue<string>("gn_text"));
            }
        }
        static OrganizationServiceProxy GetOrganizationServiceProxy(string crmUrl, string username, string password)
        {


            // Create the CRM service client credentials
            ClientCredentials credentials = new ClientCredentials();
            credentials.Windows.ClientCredential.UserName = username;
            credentials.Windows.ClientCredential.Password = password;

            // Create the CRM organization service URI
            Uri serviceUri = new Uri($"{crmUrl}/XRMServices/2011/Organization.svc");
            var result = new OrganizationServiceProxy(serviceUri, null, credentials, null);
            var whoAmIRequest = new Microsoft.Crm.Sdk.Messages.WhoAmIRequest();
            var whoAmIResponse = (Microsoft.Crm.Sdk.Messages.WhoAmIResponse)result.Execute(whoAmIRequest);


            return result;

            // Create the CRM organization service proxy
        }
    }
}

