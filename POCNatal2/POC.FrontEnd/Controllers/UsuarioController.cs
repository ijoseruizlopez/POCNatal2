using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace POC.FrontEnd.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult List()
        {
            return View("listUsuarios");
        }
       
    }
}