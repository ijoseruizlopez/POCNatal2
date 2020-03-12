using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Service.Dto
{
    public class PerfilDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
