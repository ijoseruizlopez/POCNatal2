using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Service.Dto
{
    public class CiudadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public ProvinciaDto Provincia { get; set; }
    }
}
