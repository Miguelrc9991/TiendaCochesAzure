using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TiendaCochesAzure.Models;

namespace TiendaCochesAzure.services
{
    public class ServiceSQS
    {
        private IAmazonSQS clientSQS;
        private string queueUrl;

        public ServiceSQS(IAmazonSQS client
            , IConfiguration configuration)
        {
            this.queueUrl = configuration.GetValue<string>("AWSSQSQueue");
            this.clientSQS = client;
        }

        public async Task SendMessageAsync(EmailModel email)
        {
            String data = JsonConvert.SerializeObject(email);
            SendMessageRequest request =
                new SendMessageRequest(this.queueUrl, data);
       
            SendMessageResponse response =
                await this.clientSQS.SendMessageAsync(request);
            //PODEMOS RECUPERAR EL RESULTADO Y PERSONALIZAR LA RESPUESTA
            HttpStatusCode resultado = response.HttpStatusCode;
        }

    }
}
