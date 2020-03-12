using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Service.Dto
{
    public class ProvinciaDto
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public PaisDto Pais { get; set; }

    }
}
