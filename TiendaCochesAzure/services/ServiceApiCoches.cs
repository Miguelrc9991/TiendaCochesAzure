using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGetCoches;
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
    public class ServiceApiCoches
    {
        private Uri UrlApi;
        private MediaTypeWithQualityHeaderValue Header;
        public ServiceApiCoches(string url)
        {
            this.UrlApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        public async Task<string> GetTokenAsync(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                        Email = email,
                    Contraseña = password
                };
                string json = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                string request = "/api/Auth/Login";
                HttpResponseMessage response =
                    await client.PostAsync(this.UrlApi+ request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(data);
                    string token = jObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }
        private async Task<T> CallApiAsync<T>(string request,string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header); client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
        public async Task<Models.Usuario> FindUsuarioAsync(string token)
        {
            string request = "/api/Coches/FindUsuario";
            Models.Usuario usuario = await this.CallApiAsync<Models.Usuario>(this.UrlApi + request, token);
            return usuario;
        }
        public async Task<List<Coche>> GetCochesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/getcoches";
                client.BaseAddress =this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApi + request);
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
        
       
        public async Task<string> GetEmailAsync(int idvendedor)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/getemail/"+idvendedor;
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApi + request);
                if (response.IsSuccessStatusCode)
                {
                   string email =
                        await response.Content.ReadAsAsync<string>();
                    return email;
                }
                else
                {
                    return null;
                }
            }

        }
        public async Task<List<Models.Usuario>> GetUsuariosAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/GetUsuarios";
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApi + request);
                
                if (response.IsSuccessStatusCode)
                {
                    List<Models.Usuario> usuarios =
                        await response.Content.ReadAsAsync<List<Models.Usuario>>();
                    return usuarios;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task UpdateUsuarioAsync(int idusuario, string email, string nombre, string imagen, string contraseña)
        {
            using (HttpClient client = new HttpClient())
            {
                string request ="/api​/Coches​/UpdateUsuario";
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Models.Usuario user = new Models.Usuario
                { IdUsuario=idusuario,Email=email,Nombre=nombre,Imagen=imagen,Contraseña=contraseña};
                string json = JsonConvert.SerializeObject(user);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PutAsync(this.UrlApi + request, content);
            }

        }
        public async Task DeleteUsuarioAsync(int idusuario)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/eliminarusuario/" + idusuario;

                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.DeleteAsync(this.UrlApi + request);
            }
        }
        public async Task DeleteCocheAsync(int idcoche)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/deleteCoche/​" + idcoche;
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.DeleteAsync(this.UrlApi + request);
            }
        }
        public async Task InsertUsuarioAsync
      (int idusuario,string nombre,string contraseña,string email,string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/nuevoUsuario";
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Models.Usuario usuario = new Models.Usuario();
                usuario.IdUsuario = idusuario;
                usuario.Nombre = nombre;
                usuario.Contraseña = contraseña;
                usuario.Email = email;
                usuario.Imagen = imagen;
                string json = JsonConvert.SerializeObject(usuario);
                
                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(this.UrlApi + request, content);
            }
        }

        public async Task<List<Marca>> GetMarcasAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches";
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApi + request);
                if (response.IsSuccessStatusCode)
                {
                    List<Marca> marcas =
                        await response.Content.ReadAsAsync<List<Marca>>();
                    return marcas;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<List<Modelo>> GetModelosAsync(int idmarca)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/getModelos/"+idmarca;
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApi + request);
                if (response.IsSuccessStatusCode)
                {
                    List<Modelo> modelos =
                        await response.Content.ReadAsAsync<List<Modelo>>();
                    return modelos;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<List<NuGetCoches.Version>> GetVersionesAsync(int idmarca)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/getVersiones/" + idmarca;
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApi + request);
                if (response.IsSuccessStatusCode)
                {
                    List<NuGetCoches.Version> versiones =
                        await response.Content.ReadAsAsync<List<NuGetCoches.Version>>();
                    return versiones;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<NuGetCoches.Version>> GetPreciosAsync(int idversion)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/getprecios/" + idversion;
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApi + request);
                if (response.IsSuccessStatusCode)
                {
                    List<NuGetCoches.Version> versiones =
                        await response.Content.ReadAsAsync<List<NuGetCoches.Version>>();
                    return versiones;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Coche>> GetCochesVersionAsync(int idversion)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/getcochesversion/"+idversion;
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApi + request);
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
        public async Task<List<Coche>> GetMisCochesAsync(int idusuario)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/getMisCoches/"+idusuario;
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApi + request);
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


        public async Task InsertCocheAsync
      (int idVendedor, int idVersion, string Nombre, string Descripcion, int Precio, string fileName)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Coches/NuevoCoche";
                client.BaseAddress = this.UrlApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Coche coche = new Coche();
                coche.IdVendedor = idVendedor;
                coche.IdVersion = idVersion;
                coche.Nombre = Nombre;
                coche.Descripcion = Descripcion;
                coche.Precio = Precio;
                coche.Foto1 = fileName;
                coche.Foto2 = "";
                coche.Foto3 = "";

                string json = JsonConvert.SerializeObject(coche);

                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(this.UrlApi + request, content);
            }
        }
    }
}
