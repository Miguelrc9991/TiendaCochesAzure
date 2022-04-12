using NuGetCoches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TiendaCochesAzure.services
{
    public class ServiceApiCoches
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;
        public ServiceApiCoches(string url)
        {
            this.UrlApi = url;
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data =
                        await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Coche>> GetCochesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/getcoches";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    List<Coche> coches =
                        await response.Content.ReadAsAsync<List<Coche>>();
                    return coches;
                }
                else
                {
                    return null;
                }
            }

        }
        public async Task<Usuario> GetUsuario(string email,string contraseña)
        {
            string request = "/api/Coches/"+email+"/"+contraseña;
            Usuario usuario =
                await this.CallApiAsync<Usuario>(request);
            return usuario;
        }

    }
}
