﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace POC.FrontEnd.Controllers
{
    public class CiudadController : Controller
    {
        public IActionResult List()
        {
            return View("listCiudad");
        }
    }
}