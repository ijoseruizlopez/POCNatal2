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
    public class PaisController : ControllerBase
    {
        private ServicePais _service = null;

        //Falta inyectar el servicio atraves de un conteiner
        public PaisController()
        {
            _service = new ServicePais();
        }

        [HttpGet("/api/PaisController/Get/{id}")]
        public PaisDto Get(int id)
        {
            return _service.Get(id);
        }

        [HttpPost("/api/PaisController/Save")]
        public int Save([FromBody] PaisDto pais)
        {
            _service.AddNew(pais);
            return pais.Id;
        }

        [HttpPut("/api/PaisController/Modify")]
        public void Modify(PaisDto pais)
        {
            _service.Modify(pais);
        }

        [HttpDelete("/api/PaisController/Delete/{id}")]
        public bool Delete(int id)
        {
            return _service.Delete(id);
        }

        [HttpGet("/api/PaisController/GetAll")]
        public List<PaisDto> GetAll()
        {
            return _service.GetAll();
        }
    }
}
