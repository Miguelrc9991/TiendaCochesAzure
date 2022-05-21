using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TiendaCochesAzure.Models;

namespace TiendaCochesAzure.services
{
    public class ServiceLogicApps
    {
        private MediaTypeWithQualityHeaderValue Header;
        public ServiceLogicApps()
        {
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        public async Task SendMailAsync(string email, string subject, string body)
        {
            string urlMail =
                "https://prod-165.westeurope.logic.azure.com:443/workflows/6eefda20667d4f52a0ca8ace86794374/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=pq4JBpN727br8WAIvJv097CQ4jL0S943JPt5FIeEsuE";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                EmailModel emailModel = new EmailModel
                {
                    Email = email,
                    Subject = subject,
                    Body = body
                };
                string json = JsonConvert.SerializeObject(emailModel);
           
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(urlMail, content);
            }
        }
    }
}
