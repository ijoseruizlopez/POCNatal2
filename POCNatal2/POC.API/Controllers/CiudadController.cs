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
    public class CiudadController : ControllerBase
    {

        private ServiceCiudad _service = null;

        //Falta inyectar el servicio atraves de un conteiner
        public CiudadController()
        {
            _service = new ServiceCiudad();
        }


        [HttpGet("/api/CiudadController/Get/{id}")]
        public CiudadDto Get(int id)
        {
            return _service.Get(id);
        }

        [HttpPost("/api/CiudadController/Save")]
        public int Save(CiudadDto ciudad)
        {
            _service.AddNew(ciudad);
            return ciudad.Id;
        }

        [HttpPut("/api/CiudadController/Modify")]
        public void Modify(CiudadDto ciudad)
        {
            _service.Modify(ciudad);
        }

        [HttpDelete("/api/CiudadController/Delete/{id}")]
        public bool Delete(int id)
        {
            return _service.Delete(id);
        }

        [HttpGet("/api/CiudadController/GetAll")]
        public List<CiudadDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("/api/CiudadController/GetCiudadesPorProvincia/{idProvincia}")]
        public List<CiudadDto> GetCiudadesPorProvincia(int idProvincia)
        {
            return _service.GetCiudadesPorProvincia(idProvincia);
        }

    }
}