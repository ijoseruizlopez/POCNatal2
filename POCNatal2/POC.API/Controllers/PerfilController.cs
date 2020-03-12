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
    public class PerfilController : ControllerBase
    {
        private ServicePerfil _service = null;

        //Falta inyectar el servicio atraves de un conteiner
        public PerfilController()
        {
            _service = new ServicePerfil();
        }

        [HttpGet("/api/PerfilController/Get/{id}")]
        public PerfilDto Get(int id)
        {
            return _service.Get(id);
        }

        [HttpPost("/api/PerfilController/Save")]
        public int Save([FromBody] PerfilDto perfil)
        {
            _service.AddNew(perfil);
            return perfil.Id;
        }

        [HttpPut("/api/PerfilController/Modify")]
        public void Modify(PerfilDto perfil)
        {
            _service.Modify(perfil);
        }

        [HttpDelete("/api/PerfilController/Delete/{id}")]
        public bool Delete(int id)
        {
            return _service.Delete(id);
        }

        [HttpGet("/api/PerfilController/GetAll")]
        public List<PerfilDto> GetAll()
        {
            return _service.GetAll();
        }
    }
}