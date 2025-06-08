using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace AutoTranslate
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var service = GetOrganizationServiceProxy("http://gncrm2019/Test/", "gnco\\hafezi","123@qwe");
            var sourceContext = new OrganizationServiceContext(service);
            var query = sourceContext.CreateQuery("gn_translationentry");
            
            var items = query
                .Where(x => (int)x["gn_flag"] != 1)
                .Take(10)
                .ToArray();
            foreach (var item in items)
            {
                string Text=(item.GetAttributeValue<string>("gn_text"));
                var Translation=await Translator.TranslateEnglishToPersian(Text);

                item["gn_proposed"] = Translation;
                item["gn_flag"] = 1;
                var update = new Entity
                {
                    LogicalName = item.LogicalName,
                    Id = item.Id,
                    Attributes = item.Attributes,
                };
                service.Update(update);
                
                //sourceContext.UpdateObject(item);
                //sourceContext.SaveChanges();
            }
            //items[0]["gn_flag"] = 1;
            //sourceContext.UpdateObject(items[0]);
            //sourceContext.SaveChanges();
            //service.Update(items[0]);
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

