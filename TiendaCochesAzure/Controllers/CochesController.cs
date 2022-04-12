using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGetCoches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaCochesAzure.services;

namespace TiendaCochesAzure.Controllers
{
    public class CochesController : Controller
    {
        private ServiceApiCoches service;
        public CochesController(ServiceApiCoches service)
        {
            this.service = service;
        }
        public async Task<IActionResult> GetAllCoches()
        {
            
            List<Coche> coches =
                await this.service.GetCochesAsync();
            
            return View(coches);
        }

    }
}
