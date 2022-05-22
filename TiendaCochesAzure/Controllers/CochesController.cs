using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NuGetCoches;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TiendaCochesAzure.Filters;
using TiendaCochesAzure.Models;
using TiendaCochesAzure.services;

namespace TiendaCochesAzure.Controllers
{
    public class CochesController : Controller
    {
        private ServiceApiCoches service;
        private ServiceStorageBlobs serviceb;

        private ServiceAWSS3 services3;
        private ServiceLogicApps servicela;
        private IConfiguration Configuration;

        public CochesController(ServiceApiCoches service, ServiceStorageBlobs serviceb, ServiceLogicApps servicela, ServiceAWSS3 services3, IConfiguration Configuration)
        {
            this.service = service;
            this.serviceb = serviceb;
            this.servicela = servicela;
            this.services3 = services3;
            this.Configuration = Configuration;
        }
        public async Task<IActionResult> EnviarMail(int idvendedor, string emailcomprador, string nombre)
        {
            string emailVendedor = await this.service.GetEmailAsync(idvendedor);
            string subject = "Más información del producto: " + nombre;
            string body = "Un comprador está interesado en el siguiente de sus anuncios: " + nombre +
                "</br>" +
                "Puede contactarle a su correo: " + emailcomprador;
            var client =
             new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1);
            Destination destination = new Destination();
            destination.ToAddresses = new List<string> { emailVendedor };
            Message message = new Message();
            message.Subject = new Content(subject);
            Body cuerpo = new Body();
            cuerpo.Html = new Content(body);
            cuerpo.Text = new Content(body);
            message.Body = cuerpo;
            SendEmailRequest request = new SendEmailRequest();
            //QUIEN ENVIA EL MENSAJE (NUESTRO USER VERIFIED EN SERVICIO SES)
            request.Source = "miguel.ricocorrales@gmail.com";
            request.Destination = destination;
            request.Message = message;
            SendEmailResponse response =
                await client.SendEmailAsync(request);

            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> EnviarMail(int idvendedor, string emailcomprador, string nombre)
        //{
        //    string emailVendedor = await this.service.GetEmailAsync(idvendedor);
        //    string subject = "Más información del producto: " + nombre;
        //    string body = "Un comprador está interesado en el siguiente de sus anuncios: " + nombre +
        //        "</br>" +
        //        "Puede contactarle a su correo: " + emailcomprador;
        //    //await this.servicela.SendMailAsync(emailVendedor
        //    //                , subject, body);
        //    EmailModel emailModel = new EmailModel
        //    {
        //        Email = emailVendedor,
        //        Subject = subject,
        //        Body = body
        //    };
        //    await this.servicesqs.SendMessageAsync(emailModel);

        //    return RedirectToAction("Index", "Home");

        //}
        //public async Task<IActionResult> EnviarMail()
        //{


        //    return RedirectToAction("Index", "Home");
        //}


        [AuthorizeUsuarios]

        public async Task<IActionResult> GetAllCoches()
        {
            
            List<Coche> coches =
                await this.service.GetCochesAsync();
            
            return View(coches);
        }
        [AuthorizeUsuarios]

        public async Task<IActionResult> DeleteUsuario(int idusuario)
        {

            await this.service.DeleteUsuarioAsync(idusuario);

            return RedirectToAction("GetUsuarios", "Coches");

        }
        [AuthorizeUsuarios]

        public async Task<IActionResult> DeleteCoche(int idcoche)
        {

            await this.service.DeleteCocheAsync(idcoche);

            return RedirectToAction("Index", "Home");

        }
        public async Task <IActionResult> GetMarcas()
        {
            List<Marca> marcas = await this.service.GetMarcasAsync();
            return View(marcas);
        }
        public async Task<IActionResult> GetModelos(int idmarca)
        {
            List<Modelo> modelos = await this.service.GetModelosAsync(idmarca);
            return View(modelos);
        }
        public async Task<IActionResult> GetVersiones(int idmarca)
        {
            List<NuGetCoches.Version> versiones = await this.service.GetVersionesAsync(idmarca);
            return View(versiones);
        }
        public async Task<IActionResult> GetPrecios(int idmarca)
        {
            List<NuGetCoches.Version> precios = await this.service.GetPreciosAsync(idmarca);
            return View(precios);
        }
        public async Task<IActionResult> GetCoches(int idversion)
        {
            if (HttpContext.User.Claims.Count()==0) {
                return RedirectToAction("LogIn", "Manage");
            }
            else { 
            List<Coche> coches = await this.service.GetCochesVersionAsync(idversion);
            ViewData["idversion"] = idversion;
            return View(coches);
            }
        }
        
        public async Task<IActionResult>GetMisCoches(int idusuario)
        {
            List<Coche> coches = await this.service.GetMisCochesAsync(idusuario);
            return View(coches);
        }
        [AuthorizeUsuarios]

        public IActionResult NuevoCoche(int idVersion)
        {
            ViewData["idversion"] = idVersion;
            return View();
        }

        [HttpPost]
        [AuthorizeUsuarios]
        public async Task<IActionResult> NuevoCoche(Coche coche, IFormFile file)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            Models.Usuario usuario = await this.service.FindUsuarioAsync(token);
            using (Stream stream = file.OpenReadStream())
            {
                await this.services3.UploadFileAsync(stream, file.FileName);
            }


       
            await this.service.InsertCocheAsync(
                usuario.IdUsuario,coche.IdVersion,coche.Nombre,coche.Descripcion,coche.Precio, file.FileName);
            return RedirectToAction("GetMarcas", "Coches");
        }
        public IActionResult NuevoUsuario()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoUsuario(string nombre,string contraseña,string email,IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                await this.services3.UploadFileAsync(stream, file.FileName);
            }
            await this.service.InsertUsuarioAsync(0,nombre,contraseña,email, file.FileName);
            return RedirectToAction("Login", "Manage");

        }
        public async Task<IActionResult> UpdateUsuarioAsync()
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            Models.Usuario usuario = await this.service.FindUsuarioAsync(token);
            return View(usuario);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUsuario(string nombre, string contraseña, string email, IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                await this.services3.DeleteFileAsync(file.FileName);

                await this.services3.UploadFileAsync(stream, file.FileName);
            }
            await this.service.InsertUsuarioAsync(0, nombre, contraseña, email, file.FileName);
            return RedirectToAction("Login", "Manage");
        }
        [AuthorizeUsuarios]

        public async Task<IActionResult> GetUsuarios()
        {
            List<Models.Usuario> usuarios = await this.service.GetUsuariosAsync();
            return View(usuarios);
        }
        [AuthorizeUsuarios]
        public async Task<IActionResult> Perfil()
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            Models.Usuario usuario = await this.service.FindUsuarioAsync(token);

            return View(usuario);
        }
    }
}
