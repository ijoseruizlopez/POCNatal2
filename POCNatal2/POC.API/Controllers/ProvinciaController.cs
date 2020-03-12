using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POC.Service;
using POC.Service.Dto;

namespace POC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciaController : ControllerBase
    {
        private ServiceProvincia _service = null;

        //Falta inyectar el servicio atraves de un conteiner
        public ProvinciaController()
        {
            _service = new ServiceProvincia();
        }


        [HttpGet("/api/ProvinciaController/Get/{id}")]
        public ProvinciaDto Get(int id)
        {
            return _service.Get(id);
        }

        [HttpPost("/api/ProvinciaController/Save")]
        public int Save(ProvinciaDto provincia)
        {
            _service.AddNew(provincia);
            return provincia.Id;
        }

        [HttpPut("/api/ProvinciaController/Modify")]
        public void Modify(ProvinciaDto provincia)
        {
            _service.Modify(provincia);
        }

        [HttpDelete("/api/ProvinciaController/Delete/{id}")]
        public bool Delete(int id)
        {
            return _service.Delete(id);
        }

        [HttpGet("/api/ProvinciaController/GetAll")]
        public List<ProvinciaDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("/api/ProvinciaController/GetProvinciasPorPais/{idPais}")]
        public List<ProvinciaDto> GetProvinciasPorPais(int idPais)
        {
            return _service.GetProvinciasPorPais(idPais);
        }
    }
}